namespace ReferenceGenerator.XmlDocs;

public sealed record AssemblySyntax(NameSyntax Name) : XmlDocsSyntax(XmlDocsSyntaxKind.Assembly)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
