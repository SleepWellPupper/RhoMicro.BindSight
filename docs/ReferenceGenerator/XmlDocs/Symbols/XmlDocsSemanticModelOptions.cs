namespace ReferenceGenerator.XmlDocs;

using System.Globalization;

public sealed record XmlDocsSemanticModelOptions(
    XmlDocsParserOptions XmlDocsParserOptions,
    CultureInfo PreferredCulture)
{
    public static XmlDocsSemanticModelOptions Default { get; } = new(
        XmlDocsParserOptions.Default,
        CultureInfo.InvariantCulture);
}
