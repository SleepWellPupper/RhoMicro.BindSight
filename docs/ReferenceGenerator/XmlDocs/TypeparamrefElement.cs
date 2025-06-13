namespace ReferenceGenerator.XmlDocs;

public sealed record TypeparamrefElement(
    String Name)
    : XmlDocsChildElement(XmlDocsKind.Typeparamref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
