using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;

await new AstroDocumentationService().Run();

internal class AstroDocumentationService
{
    public async Task Run()
    {
        var project = await GetProject();
        var compilation = await project.GetCompilationAsync();

        if (compilation.GetDiagnostics()
                .Where(d => d is
                    {
                        Severity: DiagnosticSeverity.Error
                    } or
                    {
                        IsWarningAsError: true
                    }).ToList() is [_, ..] errors)
        {
            return;
        }
    }

    private async Task<Project> GetProject()
    {
        var workspace = MSBuildWorkspace.Create();
        var project = await workspace.OpenProjectAsync("../RoslynNreReproTarget/RoslynNreReproTarget.csproj");
        return project;
    }
}

