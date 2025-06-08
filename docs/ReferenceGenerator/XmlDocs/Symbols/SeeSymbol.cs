namespace ReferenceGenerator.XmlDocs;

public sealed class SeeSymbol(
    SeeSyntax xmlDocsSyntax,
    XmlDocsSymbolReference reference)
    : XmlDocsSymbol<SeeSyntax>(xmlDocsSyntax, XmlDocsSymbolKind.See)
{
    public XmlDocsSymbolReference Reference => reference;
    public String Href => Syntax.Href;
    public String Langword => Syntax.Langword;

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
