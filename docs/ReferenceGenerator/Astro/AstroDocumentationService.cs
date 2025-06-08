namespace ReferenceGenerator.Astro;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using ReferenceGenerator.XmlDocs;
using RhoMicro.CodeAnalysis.Library.Text.Templating;

public class AstroDocumentationService(
    IAstroReferenceOptions options,
    ILogger<AstroDocumentationService> logger)
{
    public async Task Run(CancellationToken ct)
    {
        if (options.DryRun)
            LoggerExtensions.LogInformation(logger, "Running dry run.");

        var project = await GetProject(ct);
        var compilation = await project.GetCompilationAsync(ct);

        if (compilation is null)
            return;

        var referencePaths = new AstroReferencePathContext(compilation, options);

        var models = compilation.Assembly
            .GetPublicTypes(ct)
            .Select(t => new TypeDocumentationModel(t, compilation, XmlDocsParseNodeOptions.Default))
            .ToList();

        ClearReferenceDirectory(models, referencePaths);
        await GenerateDocs(models, referencePaths, ct);
    }

    private async Task GenerateDocs(
        List<TypeDocumentationModel> models,
        AstroReferencePathContext referencePaths,
        CancellationToken ct)
    {
        foreach (var model in models)
            await GenerateAstroDocs(model, referencePaths, ct);
    }

    private void ClearReferenceDirectory(List<TypeDocumentationModel> models, AstroReferencePathContext referencePaths)
    {
        if (options.DryRun || !options.ClearReferenceDirectory)
            return;

        var directories = models
            .Select(m => referencePaths.GetPaths(m.Type).ContainingDirectory)
            .ToHashSet()
            .Where(Directory.Exists);

        foreach (var directory in directories)
        {
            logger.LogInformation("Clearing reference directory '{Directory}'", directory);
            Directory.Delete(directory!, recursive: true);
        }
    }

    private async Task GenerateAstroDocs(
        TypeDocumentationModel model,
        AstroReferencePathContext referencePaths,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var paths = referencePaths.GetPaths(model.Type);
        LoggerExtensions.LogInformation(logger, "Creating astro file '{FilePath}'.", paths.AbsoluteFilePath);

        if (options.DryRun)
            return;

        if (!Directory.Exists(paths.ContainingDirectory))
            Directory.CreateDirectory(paths.ContainingDirectory);

        var content = TemplateRenderer.Render(
            new AstroTypeReferenceTemplate(
                model,
                referencePaths),
            ct);

        await File.WriteAllTextAsync(paths.AbsoluteFilePath, content, ct);
    }

    private async Task<Project> GetProject(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var workspace = MSBuildWorkspace.Create();
        LoggerExtensions.LogInformation(logger, "Loading Project.");
        var project = await workspace.OpenProjectAsync(
            options.ProjectPath,
            new Progress<ProjectLoadProgress>(p => LoggerExtensions.LogInformation(logger, "{Operation}", p.Operation)),
            ct);
        LoggerExtensions.LogInformation(logger, "Loaded project.");

        return project;
    }
}

file static class Extensions
{
    public static List<INamedTypeSymbol> GetPublicTypes(
        this IAssemblySymbol assembly,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = new List<INamedTypeSymbol>();
        assembly.GlobalNamespace.GetPublicTypes(result, ct);

        return result;
    }

    private static void GetPublicTypes(
        this INamespaceSymbol @namespace,
        List<INamedTypeSymbol> types,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        foreach (var type in @namespace.GetTypeMembers())
        {
            ct.ThrowIfCancellationRequested();

            if (type.DeclaredAccessibility is Accessibility.Public)
                types.Add(type);
        }

        foreach (var childNamespace in @namespace.GetNamespaceMembers())
            childNamespace.GetPublicTypes(types, ct);
    }
}
