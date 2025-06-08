namespace ReferenceGenerator.XmlDocs;

public sealed record TypeparamrefSyntax(String Name) : XmlDocsSyntax(XmlDocsSyntaxKind.Typeparamref)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
