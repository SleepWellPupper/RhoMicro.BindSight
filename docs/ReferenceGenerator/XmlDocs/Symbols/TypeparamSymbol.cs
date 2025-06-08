namespace ReferenceGenerator.XmlDocs;

public sealed class TypeparamSymbol(
    TypeparamSyntax syntax,
    XmlDocsSymbolReference reference)
    : XmlDocsSymbol<TypeparamSyntax>(syntax, XmlDocsSymbolKind.Typeparam)
{
    public XmlDocsSymbolReference Reference => reference;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
