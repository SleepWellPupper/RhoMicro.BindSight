namespace ReferenceGenerator.XmlDocs;

public sealed record MemberSyntax(
    String Name,
    InheritdocSyntax? Inheritdoc,
    SyntaxChildren<MainElementSyntax> Elements)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Member)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
