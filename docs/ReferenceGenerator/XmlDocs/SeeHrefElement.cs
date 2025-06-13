namespace ReferenceGenerator.XmlDocs;

public sealed record SeeHrefElement(
    String Href)
    : XmlDocsChildElement(XmlDocsKind.SeeHref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
