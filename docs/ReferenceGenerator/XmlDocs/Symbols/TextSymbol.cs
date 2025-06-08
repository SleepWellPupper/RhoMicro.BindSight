namespace ReferenceGenerator.XmlDocs;

public sealed class TextSymbol(TextSyntax xmlDocsSyntax)
    : XmlDocsSymbol<TextSyntax>(xmlDocsSyntax, XmlDocsSymbolKind.Text)
{
    public String Text => Syntax.Text;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
