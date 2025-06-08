namespace ReferenceGenerator.XmlDocs;

public sealed record ParamrefSyntax(String Name) : XmlDocsSyntax(XmlDocsSyntaxKind.Paramref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
