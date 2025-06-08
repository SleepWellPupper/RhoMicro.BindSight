namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;
using System.Numerics;

[Flags]
public enum DiagnosticKinds : UInt32
{
    None = 0,
    UnrecognizedMainElement = 1 << 0,
    UnrecognizedNestedElement = 1 << 1,
    Default = None,
    All = UnrecognizedMainElement | UnrecognizedNestedElement,
}
