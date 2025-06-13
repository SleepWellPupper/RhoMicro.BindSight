namespace ReferenceGenerator.XmlDocs;

public sealed record ParamElement(
    String Name,
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsElement(XmlDocsKind.Param)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
