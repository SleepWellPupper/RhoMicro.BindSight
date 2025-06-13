namespace ReferenceGenerator.Astro;

using System.Text;
using Microsoft.CodeAnalysis;
using XmlDocs;

internal static class SymbolExtensions
{
    public static String ToInheritanceTreeString(
        INamedTypeSymbol type,
        XmlDocsContext docsContext,
        AstroReferencePathsContext referencePaths)
    {
        var builder = new StringBuilder();
        AppendInheritance(
            type,
            docsContext,
            referencePaths,
            builder,
            new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default),
            isLast: false,
            indentation: " - ");
        var result = builder.ToString();

        return result;
    }

    private static void AppendInheritance(
        INamedTypeSymbol type,
        XmlDocsContext docsContext,
        AstroReferencePathsContext referencePaths,
        StringBuilder builder,
        HashSet<INamedTypeSymbol> handledTypes,
        Boolean isLast,
        ReadOnlySpan<Char> indentation)
    {
        builder.AppendMarkdownLink(type, docsContext, referencePaths);
        var inheritedTypes = new List<INamedTypeSymbol>();

        if (type.BaseType is { } baseType && handledTypes.Add(baseType))
            inheritedTypes.Add(baseType);

        inheritedTypes.AddRange(
            type.Interfaces.Where(
                    handledTypes.Add)
                .OrderBy(
                    t => t.ToDisplayString(SymbolDisplayFormats.FullyQualifiedNamespaceOmitted)));

        ReadOnlySpan<Char> branch = " - ";
        ReadOnlySpan<Char> leaf = " - ";
        ReadOnlySpan<Char> edge = "   ";
        ReadOnlySpan<Char> empty = "   ";

        Span<Char> newIndentation = stackalloc Char[indentation.Length + 3];
        if (indentation.Length > 2)
        {
            indentation[..^3].CopyTo(newIndentation);
            (isLast ? empty : edge).CopyTo(newIndentation[(indentation.Length - 3)..]);
        }

        for (var i = 0; i < inheritedTypes.Count; i++)
        {
            var inheritedType = inheritedTypes[i];
            var isLastType = i == inheritedTypes.Count - 1;
            if (isLastType)
            {
                leaf.CopyTo(newIndentation[^3..]);
            }
            else
            {
                branch.CopyTo(newIndentation[^3..]);
            }

            AppendInheritance(
                inheritedType,
                docsContext,
                referencePaths,
                builder,
                handledTypes,
                isLastType,
                newIndentation);
        }
    }

    public static StringBuilder AppendMarkdownLink(
        this StringBuilder builder,
        ISymbol symbol,
        XmlDocsContext docsContext,
        AstroReferencePathsContext referencePaths,
        Boolean applyTypeClass = true)
    {
        var name = symbol.ToDisplayString(SymbolDisplayFormats.SimpleGenericName)
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");

        var normalizedSymbol =
            symbol switch
            {
                INamedTypeSymbol { IsGenericType: true } genericType => genericType.ConstructedFrom,
                IMethodSymbol { IsGenericMethod: true } genericMethod => genericMethod.ConstructedFrom,
                IPropertySymbol => symbol,
                { ContainingSymbol: { } containingSymbol } => containingSymbol,
                _ => symbol
            };

        var href = referencePaths.GetPaths(normalizedSymbol).AnchorHref.AsString;
        builder.Append(@"<span title=""");
        var titleBuilder = new StringBuilder();
        docsContext.Provider.GetMemberXmlDocs(normalizedSymbol)
            .Summary
            .Accept(new HtmlTitleStringVisitor(titleBuilder, docsContext));
        builder.Append(titleBuilder.ToString().Trim());
        builder.Append(@"""><a");

        if (applyTypeClass && normalizedSymbol switch
            {
                INamedTypeSymbol { TypeKind: TypeKind.Interface } => "cs-interface",
                INamedTypeSymbol { TypeKind: TypeKind.Struct } => "cs-struct",
                INamedTypeSymbol => "cs-class",
                _ => null
            } is { } @class)
        {
            builder.Append($" class=\"{@class}\"");
        }

        builder.Append($""" href="{href}">{name}</a></span>""");

        return builder;
    }
}
