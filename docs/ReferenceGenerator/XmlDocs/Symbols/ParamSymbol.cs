namespace ReferenceGenerator.XmlDocs;

public sealed class ParamSymbol(
    ParamSyntax syntax,
    XmlDocsSymbolReference reference)
    : XmlDocsSymbol<ParamSyntax>(syntax, XmlDocsSymbolKind.Param)
{
    public XmlDocsSymbolReference Reference => reference;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
