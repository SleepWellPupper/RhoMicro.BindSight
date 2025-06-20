namespace ReferenceGenerator.Astro;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

internal sealed class AstroReferencePathsContext(
    Compilation compilation,
    IAstroReferenceOptions options,
    ILogger<AstroReferencePathsContext> logger)
{
    private readonly ConcurrentDictionary<ISymbol, AstroReferencePaths> _paths =
        new(SymbolEqualityComparer.Default);

    private const Char Separator = '_';
    public AstroReferencePaths GetPaths(ISymbol symbol) => _paths.GetOrAdd(symbol, CreatePaths);

    private AstroReferencePaths CreatePaths(ISymbol symbol) =>
        IsExternal(symbol)
            ? CreateExternalPaths(symbol)
            : symbol.DeclaredAccessibility is not Accessibility.Public
                ? AstroReferencePaths.Empty
                : symbol is INamedTypeSymbol type
                    ? CreateTypePaths(type)
                    : CreateMemberPaths(symbol);

    private Boolean IsExternal(ISymbol symbol)
    {
        var rootNamespace = getRootNamespace(symbol.ContainingNamespace);
        var isExternal = rootNamespace is not "RhoMicro"
                         || rootNamespace is "Microsoft" or "System"
                         || !SymbolEqualityComparer.Default.Equals(symbol.ContainingAssembly, compilation.Assembly);

        if (isExternal)
            logger.LogInformation(
                "external: '{Symbol}'",
                symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        else
            logger.LogInformation(
                "internal: '{Symbol}'",
                symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        return isExternal;

        String getRootNamespace(INamespaceSymbol @namespace)
        {
            while (true)
            {
                if (@namespace.IsGlobalNamespace)
                    return String.Empty;
                if (@namespace.ContainingNamespace is { IsGlobalNamespace: true })
                    return @namespace.Name;
                @namespace = @namespace.ContainingNamespace;
            }
        }
    }

    private AstroReferencePaths CreateExternalPaths(ISymbol symbol)
    {
        var result = new AstroReferencePaths(
            AnchorHref: AnchorHref.CreateExternal(symbol, options),
            AbsoluteFilePath: String.Empty,
            ContainingDirectory: String.Empty);

        return result;
    }

    private AstroReferencePaths CreateMemberPaths(ISymbol symbol)
    {
        var anchorHref = AnchorHref.Create(symbol, options);

        var result = new AstroReferencePaths(
            AbsoluteFilePath: String.Empty,
            ContainingDirectory: String.Empty,
            AnchorHref: anchorHref);

        return result;
    }

    private AstroReferencePaths CreateTypePaths(INamedTypeSymbol type)
    {
        var stringBuilder = new StringBuilder();
        AppendAstroNamespace(type.ContainingNamespace, stringBuilder.Clear());
        var directoryName = stringBuilder.ToString();

        var anchorHref = AnchorHref.CreateForType(type, options);
        var absoluteFilePath = AbsoluteFilePath.Create(type, options);

        var containingDirectory = Path.Combine(
            Environment.CurrentDirectory,
            options.ReferenceDirectory,
            directoryName);

        var result = new AstroReferencePaths(
            AbsoluteFilePath: absoluteFilePath,
            ContainingDirectory: containingDirectory,
            AnchorHref: anchorHref);

        return result;
    }

    private static void AppendAstroIdParameterType(ITypeSymbol type, StringBuilder stringBuilder)
    {
        if (type.ContainingType is { } parent)
        {
            AppendAstroIdParameterType(parent, stringBuilder);
            stringBuilder.Append(Separator);
        }

        if (type is IArrayTypeSymbol arrayType)
            AppendAstroIdParameterArrayType(arrayType, stringBuilder);

        if (type is INamedTypeSymbol namedType)
            AppendAstroIdParameterNamedType(namedType, stringBuilder);

        if (type is ITypeParameterSymbol typeParameter)
            AppendAstroIdParameterTypeParameter(typeParameter, stringBuilder);

        if (type is IDynamicTypeSymbol dynamicParameter)
            AppendAstroIdParameterDynamicType(dynamicParameter, stringBuilder);
    }

    private static void AppendAstroIdParameterDynamicType(
        IDynamicTypeSymbol dynamicParameter,
        StringBuilder stringBuilder)
    {
        stringBuilder.Append(dynamicParameter.Name);
    }

    private static void AppendAstroIdParameterTypeParameter(
        ITypeParameterSymbol typeParameter,
        StringBuilder stringBuilder)
    {
        stringBuilder.Append(typeParameter.Name);
    }

    private static void AppendAstroIdParameterNamedType(
        INamedTypeSymbol type,
        StringBuilder stringBuilder)
    {
        AppendAstroIdParameterTypeNamespace(type, stringBuilder);

        stringBuilder.Append($"{type.Name}{Separator}{type.Arity}");

        if (type.TypeArguments is not [_, ..] args)
            return;

        AppendAstroIdGenericTypeArguments(stringBuilder, args);
    }

    private static void AppendAstroIdGenericTypeArguments(StringBuilder stringBuilder, ImmutableArray<ITypeSymbol> args)
    {
        stringBuilder.Append($"{Separator}0g0{Separator}");

        for (var i = 0; i < args.Length; i++)
        {
            if (i > 0)
                stringBuilder.Append(Separator);

            AppendAstroIdParameterType(args[i], stringBuilder);
        }

        stringBuilder.Append($"{Separator}0g1");
    }

    private static void AppendAstroIdParameterTypeNamespace(ITypeSymbol type, StringBuilder stringBuilder)
    {
        AppendAstroNamespace(type.ContainingNamespace, stringBuilder);
        if (!type.ContainingNamespace.IsGlobalNamespace)
            stringBuilder.Append(Separator);
    }

    private static void AppendAstroIdParameterArrayType(IArrayTypeSymbol array, StringBuilder stringBuilder)
    {
        stringBuilder.Append("-0a0-");
        AppendAstroIdParameterType(array.ElementType, stringBuilder);
        stringBuilder.Append($"-{array.Rank}-0a1");
    }

    private static void AppendAstroNamespace(
        INamespaceSymbol @namespace,
        StringBuilder stringBuilder)
    {
        if (@namespace.IsGlobalNamespace)
            return;

        if (@namespace.ContainingNamespace is { IsGlobalNamespace: false } parent)
        {
            AppendAstroNamespace(parent, stringBuilder);
            stringBuilder.Append('.');
        }

        stringBuilder.Append(@namespace.Name);
    }
}
