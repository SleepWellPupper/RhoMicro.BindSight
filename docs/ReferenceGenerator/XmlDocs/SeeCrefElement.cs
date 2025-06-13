namespace ReferenceGenerator.XmlDocs;

public sealed record SeeCrefElement(
    String Cref)
    : XmlDocsChildElement(XmlDocsKind.SeeCref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
