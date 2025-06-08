namespace ReferenceGenerator.XmlDocs;

public sealed record SeeSyntax(String Cref = "", String Href = "", String Langword = "") : XmlDocsSyntax(XmlDocsSyntaxKind.See)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
