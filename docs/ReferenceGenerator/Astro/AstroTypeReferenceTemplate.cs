namespace ReferenceGenerator.Astro;

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;
using XmlDocs;

[Template(
    """
    ---
    title: (:model.DisplayName:)
    description: The reference documentation for the (:model.Namespace:).(:model.DisplayName:) type.
    ---

    import { FileTree } from '@astrojs/starlight/components';

    ```cs
    (:model.Signature:)
    ```

    Namespace: (:model.Namespace:)

    Inheritance:
    <FileTree>
    (:Inheritance:)
    </FileTree>

    (:Markdown:)

    {:
        foreach(var group in MemberGroups)
        {
            (:group:)
        }
    :}

    """, Usings = ["Microsoft.CodeAnalysis.CSharp"], CancellationTokenParameterName = "ct"
)]
internal readonly partial struct AstroTypeReferenceTemplate(
    TypeDocumentationModel model,
    AstroReferencePathsContext referencePaths)
{
    private String Markdown => model.GetMarkdownString(referencePaths);

    private ImmutableArray<AstroMemberGroupReferenceTemplate> MemberGroups { get; } =
    [
        new AstroMemberGroupReferenceTemplate(model.Constructors, "Constructors", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Fields, "Fields", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Events, "Events", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Properties, "Properties", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Methods, "Methods", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Operators, "Operators", referencePaths),
    ];

    private String Inheritance { get; } =
        CreateInheritance(model.Type, model.DocsContext, referencePaths);

    private static String CreateInheritance(
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
            new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default));
        var result = builder.ToString();

        return result;
    }

    private static void AppendInheritance(
        INamedTypeSymbol type,
        XmlDocsContext docsContext,
        AstroReferencePathsContext referencePaths,
        StringBuilder builder,
        HashSet<INamedTypeSymbol> handledTypes,
        Int32 depth = 0)
    {
        builder
            .Append(' ', depth)
            .Append("- ")
            .AppendMarkdownLink(type, docsContext, referencePaths)
            .AppendLine();
        var inheritedTypes = new List<INamedTypeSymbol>();

        if (type.BaseType is { } baseType
            && handledTypes.Add(baseType)
           )
            inheritedTypes.Add(baseType);

        inheritedTypes.AddRange(
            type.Interfaces
                .Where(handledTypes.Add)
                .OrderBy(
                    t => t.ToDisplayString(SymbolDisplayFormats.FullyQualifiedNamespaceOmitted)));

        foreach (var inheritedType in inheritedTypes)
        {
            AppendInheritance(
                inheritedType,
                docsContext,
                referencePaths,
                builder,
                handledTypes,
                depth + 2);
        }
    }
}
