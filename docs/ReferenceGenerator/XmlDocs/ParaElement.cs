namespace ReferenceGenerator.XmlDocs;

public sealed record ParaElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsChildElement(XmlDocsKind.Para)
{
    public static ParaElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
