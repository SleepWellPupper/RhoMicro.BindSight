namespace ReferenceGenerator.Astro;

using RhoMicro.CodeAnalysis;

[Template(
    """
    ##### (:Header:)

    ```cs
    (:model.Signature:)
    ```

    (:Markdown:)

    """, CancellationTokenParameterName = "ct")]
internal readonly partial struct AstroMemberReferenceTemplate(
    MemberDocumentationModel model,
    AstroReferencePathsContext references)
{
    private String Markdown => model.GetMarkdownString(references);

    private String Header =>
        model.Symbol.ToDisplayString(SymbolDisplayFormats.HeaderSignature)
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");

    private String Link =>
        references.GetPaths(model.Symbol).AnchorHref.Value;
}
