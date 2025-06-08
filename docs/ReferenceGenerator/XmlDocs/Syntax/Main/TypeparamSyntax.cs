namespace ReferenceGenerator.XmlDocs;

public sealed record TypeparamSyntax(
    String Name,
    NestedElementsOrInheritdocSyntax Child)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Typeparam)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
