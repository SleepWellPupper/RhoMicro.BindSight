namespace ReferenceGenerator.Astro;

using System.Collections.Immutable;
using RhoMicro.CodeAnalysis;

[Template(
    """
    ---
    title: (:model.TypeKindDisplay:) (:model.DisplayName:)
    description: The reference documentation for the (:model.Namespace:).(:model.DisplayName:) type.
    ---

    (:new AstroDocumentationNodeTemplate(model.Documentation.GetSummary(ct), referencePaths):)

    ```cs
    (:model.Signature:)
    ```

    Namespace: (:model.Namespace:)


    ## Members

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
    AstroReferencePathContext referencePaths)
{
    private ImmutableArray<AstroMemberGroupReferenceTemplate> MemberGroups { get; } =
    [
        new AstroMemberGroupReferenceTemplate(model.Constructors, "Constructors", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Fields, "Fields", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Events, "Events", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Properties, "Properties", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Methods, "Methods", referencePaths),
        new AstroMemberGroupReferenceTemplate(model.Operators, "Operators", referencePaths),
    ];
}
