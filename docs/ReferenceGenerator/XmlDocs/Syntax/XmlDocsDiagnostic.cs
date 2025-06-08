namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics;
using System.Globalization;

public readonly record struct XmlDocsDiagnostic(
    DiagnosticKinds Kind,
    String Message)
{
    private static String GetMessageFormat(DiagnosticKinds kind)
        => kind switch
        {
            DiagnosticKinds.UnrecognizedNestedElement => "Unrecognized element: '{0}'.",
            DiagnosticKinds.UnrecognizedMainElement => "Unrecognized element: '{0}'.",
            _ => throw new UnreachableException($"Unable to determine message format for {kind}.")
        };

    public static XmlDocsDiagnostic Create(
        DiagnosticKinds kind,
        IFormatProvider formatProvider,
        params ReadOnlySpan<Object?> arguments)
    {
        AssertSingleKind(kind);

        var format = GetMessageFormat(kind);
        var message = String.Format(formatProvider, format, arguments);
        var result = new XmlDocsDiagnostic(kind, message);

        return result;
    }

    private static void AssertSingleKind(DiagnosticKinds kind)
    {
        if (kind.GetPopCount() != 1)
            throw new ArgumentOutOfRangeException(
                nameof(kind),
                kind,
                $"{nameof(kind)} must be single kind");
    }
}
