namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.CodeAnalysis;

public sealed record XmlDocsParserOptions(
    IFormatProvider ErrorFormatProvider,
    DiagnosticKinds SupportedErrors = DiagnosticKinds.Default,
    DiagnosticKinds SupportedWarnings = DiagnosticKinds.Default,
    Boolean TrimSyntheticWhitespace = true,
    String SyntheticWhitespace = "\n    ",
    Boolean ThrowXmlExceptions = false)
{
    public static XmlDocsParserOptions Default { get; } = new(
        ErrorFormatProvider: CultureInfo.InvariantCulture);
}
