namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record RemarksElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsElement(XmlDocsKind.Remarks)
{
    public static RemarksElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
