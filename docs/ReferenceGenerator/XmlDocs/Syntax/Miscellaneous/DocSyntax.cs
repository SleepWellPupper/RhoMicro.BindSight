namespace ReferenceGenerator.XmlDocs;

public sealed record DocSyntax(
    AssemblySyntax Assembly,
    MembersSyntax Members)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Doc)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
