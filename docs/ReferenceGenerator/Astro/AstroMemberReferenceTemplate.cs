namespace ReferenceGenerator.Astro;

using RhoMicro.CodeAnalysis;

[Template(
    """
    #### (:model.Symbol.GetMemberName():) <a id="(:Id:)"></a>

    (:new AstroDocumentationNodeTemplate(model.Documentation.GetSummary(ct), referencePaths):)

    ```cs
    (:model.Signature:)
    ```

    """, CancellationTokenParameterName = "ct")]
internal readonly partial struct AstroMemberReferenceTemplate(
    MemberDocumentationModel model,
    AstroReferencePathContext referencePaths)
{
    private String Id => referencePaths.GetPaths(model.Symbol).HeadingId;
}
