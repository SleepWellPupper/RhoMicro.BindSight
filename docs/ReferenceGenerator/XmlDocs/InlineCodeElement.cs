namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record InlineCodeElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsChildElement(XmlDocsKind.InlineCode)
{
    public static InlineCodeElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
