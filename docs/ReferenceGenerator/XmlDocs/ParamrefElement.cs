namespace ReferenceGenerator.XmlDocs;

public sealed record ParamrefElement(String Name) : XmlDocsChildElement(XmlDocsKind.Paramref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
