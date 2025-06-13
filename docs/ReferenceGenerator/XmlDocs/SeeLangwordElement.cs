namespace ReferenceGenerator.XmlDocs;

public sealed record SeeLangwordElement(
    String Langword)
    : XmlDocsChildElement(XmlDocsKind.SeeLangword)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
