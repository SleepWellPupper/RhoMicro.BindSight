namespace ReferenceGenerator.Astro;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Primitives;
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

    private static String GetExternalLinkIcon(String colorClass) =>
        $"""<svg class="{colorClass}" style="display: inline-block; width: 1em; height: 1em;" viewbox="0 0 48 48"><path d="M36 24c-1.2 0-2 0.8-2 2v12c0 1.2-0.8 2-2 2h-22c-1.2 0-2-0.8-2-2v-22c0-1.2 0.8-2 2-2h12c1.2 0 2-0.8 2-2s-0.8-2-2-2h-12c-3.4 0-6 2.6-6 6v22c0 3.4 2.6 6 6 6h22c3.4 0 6-2.6 6-6v-12c0-1.2-0.8-2-2-2z"></path><path d="M43.8 5.2c-0.2-0.4-0.6-0.8-1-1-0.2-0.2-0.6-0.2-0.8-0.2h-12c-1.2 0-2 0.8-2 2s0.8 2 2 2h7.2l-18.6 18.6c-0.8 0.8-0.8 2 0 2.8 0.4 0.4 0.8 0.6 1.4 0.6s1-0.2 1.4-0.6l18.6-18.6v7.2c0 1.2 0.8 2 2 2s2-0.8 2-2v-12c0-0.2 0-0.6-0.2-0.8z"></path></svg>""";

    public static StringBuilder AppendMarkdownLink(
        this StringBuilder builder,
        ISymbol symbol,
        XmlDocsContext docsContext,
        AstroReferencePathsContext referencePaths,
        Boolean applyTypeClass = true)
    {
        var summary = docsContext.Provider.GetMemberXmlDocs(symbol).Summary;

        var name = symbol.ToDisplayString(SymbolDisplayFormats.SimpleGenericName)
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");

        var normalizedSymbol =
            symbol switch
            {
                INamedTypeSymbol { IsGenericType: true } genericType => genericType.ConstructedFrom,
                IMethodSymbol { IsGenericMethod: true } genericMethod => genericMethod.ConstructedFrom,
                IPropertySymbol => symbol,
                ITypeParameterSymbol or IParameterSymbol => symbol.ContainingSymbol,
                { ContainingType: { } containingType } => containingType,
                _ => symbol
            };

        var href = referencePaths.GetPaths(normalizedSymbol).AnchorHref is
        {
            IsExternal: false,
        } nonExternal
            ? nonExternal
            : referencePaths.GetPaths(symbol).AnchorHref;

        var @class = applyTypeClass
            ? normalizedSymbol switch
            {
                INamedTypeSymbol t => t.TypeKind switch
                {
                    TypeKind.Interface => "cs-interface",
                    TypeKind.Struct => "cs-struct",
                    TypeKind.Class or TypeKind.Array or TypeKind.Delegate => "cs-class",
                    TypeKind.Enum => "cs-enum",
                    _ => "cs-member"
                },
                _ => "cs-member"
            }
            : String.Empty;

        builder.Append(@"<span title=""");
        summary.Accept(new HtmlTitleStringVisitor(builder, docsContext));
        builder.Append(@"""><a");

        builder.Append($""" class="{@class}" href="{href.Value}" """);

        if (href.IsExternal)
            builder.Append(" target=\"_blank\"");

        builder.Append($">{name}");

        if (href.IsExternal)
            builder.Append(GetExternalLinkIcon(@class));

        builder.Append("</a></span>");

        return builder;
    }
}
