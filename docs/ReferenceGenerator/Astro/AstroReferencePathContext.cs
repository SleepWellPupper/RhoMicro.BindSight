namespace ReferenceGenerator.Astro;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

internal sealed class AstroReferencePathContext(Compilation compilation, IAstroReferenceOptions options)
{
    private readonly ConcurrentDictionary<ISymbol, AstroReferencePaths> _paths =
        new(SymbolEqualityComparer.Default);

    public AstroReferencePaths GetPaths(ISymbol symbol) => _paths.GetOrAdd(symbol, CreatePaths);

    private AstroReferencePaths CreatePaths(ISymbol symbol) =>
        SymbolEqualityComparer.Default.Equals(symbol.ContainingAssembly, compilation.Assembly)
            ? symbol is INamedTypeSymbol
            {
                DeclaredAccessibility: Accessibility.Public
            } type
                ? CreateTypePaths(type)
                : symbol is
                {
                    DeclaredAccessibility: Accessibility.Public, ContainingType: { } containingType
                }
                    ? CreateMemberPaths(symbol, containingType)
                    : AstroReferencePaths.Empty
            : CreateExternalPaths(symbol);

    private AstroReferencePaths CreateExternalPaths(ISymbol symbol)
    {
        var externalReferencePath = String.Format(
            options.ExternalReferenceUrlFormat,
            Uri.EscapeDataString(symbol.ToDisplayString(SymbolDisplayFormats.FullyQualifiedNamespaceOmitted)));

        var result = new AstroReferencePaths(
            AnchorHref: externalReferencePath,
            AbsoluteFilePath: externalReferencePath,
            ContainingDirectory: String.Empty,
            HeadingId: String.Empty);

        return result;
    }

    private AstroReferencePaths CreateMemberPaths(
        ISymbol symbol,
        INamedTypeSymbol containingType)
    {
        var stringBuilder = new StringBuilder();
        AppendAstroFragment(symbol, containingType, stringBuilder);
        var fragment = stringBuilder.ToString();
        AppendAstroNamespace(symbol.ContainingNamespace, stringBuilder.Clear(), '.');
        var directoryName = stringBuilder.ToString();
        var anchorHref = Path.Combine(options.RelativeReferencePathBase, directoryName, fragment);

        AppendAstroId(symbol, stringBuilder.Clear());
        var headingId = stringBuilder.ToString();

        var result = new AstroReferencePaths(
            AbsoluteFilePath: String.Empty,
            ContainingDirectory: String.Empty,
            HeadingId: headingId,
            AnchorHref: anchorHref);

        return result;
    }

    private AstroReferencePaths CreateTypePaths(INamedTypeSymbol type)
    {
        var stringBuilder = new StringBuilder();
        AppendAstroId(type, stringBuilder);
        var headingId = stringBuilder.ToString();

        AppendAstroNamespace(type.ContainingNamespace, stringBuilder.Clear(), separator: '.');
        var directoryName = stringBuilder.ToString();

        var anchorHref = Path.Combine(options.RelativeReferencePathBase, directoryName, headingId);

        var containingDirectory = Path.Combine(
            Environment.CurrentDirectory,
            options.ReferenceDirectory,
            directoryName);

        AppendAstroType(type, stringBuilder.Clear());
        var absoluteFilePath = Path.Combine(
            containingDirectory,
            $"{stringBuilder}.md");

        var result = new AstroReferencePaths(
            AbsoluteFilePath: absoluteFilePath,
            ContainingDirectory: containingDirectory,
            HeadingId: headingId,
            AnchorHref: anchorHref);

        return result;
    }

    private static void AppendAstroId(ISymbol member, StringBuilder stringBuilder)
    {
        stringBuilder.Append(member.Name);

        if (member is not IMethodSymbol m)
            return;

        stringBuilder.Append($"-{m.Arity}");

        if (m.Parameters is [_, ..] @params)
            AppendAstroIdParameters(stringBuilder, @params);
    }

    private static void AppendAstroIdParameters(StringBuilder stringBuilder, ImmutableArray<IParameterSymbol> @params)
    {
        stringBuilder.Append("-0p0-");

        for (var i = 0; i < @params.Length; i++)
        {
            if (i > 0)
                stringBuilder.Append('_');

            AppendAstroIdParameterType(@params[i].Type, stringBuilder);
        }

        stringBuilder.Append("-0p1");
    }

    private static void AppendAstroIdParameterType(ITypeSymbol type, StringBuilder stringBuilder)
    {
        if (type.ContainingType is { } parent)
        {
            AppendAstroIdParameterType(parent, stringBuilder);
            stringBuilder.Append('-');
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

        stringBuilder.Append($"{type.Name}-{type.Arity}");

        if (type.TypeArguments is not [_, ..] args)
            return;

        AppendAstroIdGenericTypeArguments(stringBuilder, args);
    }

    private static void AppendAstroIdGenericTypeArguments(StringBuilder stringBuilder, ImmutableArray<ITypeSymbol> args)
    {
        stringBuilder.Append("-0g0-");

        for (var i = 0; i < args.Length; i++)
        {
            if (i > 0)
                stringBuilder.Append('_');

            AppendAstroIdParameterType(args[i], stringBuilder);
        }

        stringBuilder.Append("-0g1");
    }

    private static void AppendAstroIdParameterTypeNamespace(ITypeSymbol type, StringBuilder stringBuilder)
    {
        AppendAstroNamespace(type.ContainingNamespace, stringBuilder, separator: '-');
        if (!type.ContainingNamespace.IsGlobalNamespace)
            stringBuilder.Append('-');
    }

    private static void AppendAstroIdParameterArrayType(IArrayTypeSymbol array, StringBuilder stringBuilder)
    {
        stringBuilder.Append("-0a0-");
        AppendAstroIdParameterType(array.ElementType, stringBuilder);
        stringBuilder.Append($"-{array.Rank}-0a1");
    }

    private static void AppendAstroFragment(
        ISymbol member,
        INamedTypeSymbol containingType,
        StringBuilder stringBuilder)
    {
        AppendAstroType(containingType, stringBuilder);
        stringBuilder.Append('#');
        AppendAstroId(member, stringBuilder);
    }

    // namespace-namespace-namespace
    private static void AppendAstroNamespace(
        INamespaceSymbol @namespace,
        StringBuilder stringBuilder,
        Char separator)
    {
        if (@namespace.IsGlobalNamespace)
            return;

        if (@namespace.ContainingNamespace is { IsGlobalNamespace: false } parent)
        {
            AppendAstroNamespace(parent, stringBuilder, separator);
            stringBuilder.Append(separator);
        }

        stringBuilder.Append(@namespace.Name);
    }

    // typename-arity-nestedtypename-arity
    private static void AppendAstroType(INamedTypeSymbol type, StringBuilder stringBuilder)
    {
        if (type.ContainingType is { } parent)
        {
            AppendAstroType(parent, stringBuilder);
            stringBuilder.Append('-');
        }

        stringBuilder.Append($"{type.Name}-{type.Arity}");
    }
}
