namespace ReferenceGenerator.XmlDocs;

public sealed record NameSyntax(
    TextSyntax Text)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Name)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
