namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record ReturnsElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsElement(XmlDocsKind.Returns)
{
    public static ReturnsElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
