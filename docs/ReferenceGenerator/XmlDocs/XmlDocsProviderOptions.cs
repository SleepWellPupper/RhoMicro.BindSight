namespace ReferenceGenerator.XmlDocs;

using System.Globalization;

public sealed record XmlDocsProviderOptions(
    CultureInfo PreferredCulture,
    String TrimmableWhitespace = "\n    ")
{
    public static XmlDocsProviderOptions Default { get; } = new(
        CultureInfo.InvariantCulture);
}
