namespace ReferenceGenerator.XmlDocs;

public sealed class ParamrefSymbol(
    ParamrefSyntax xmlDocsSyntax,
    XmlDocsSymbolReference reference)
    : XmlDocsSymbol<ParamrefSyntax>(xmlDocsSyntax, XmlDocsSymbolKind.Paramref)
{
    public XmlDocsSymbolReference Reference => reference;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
