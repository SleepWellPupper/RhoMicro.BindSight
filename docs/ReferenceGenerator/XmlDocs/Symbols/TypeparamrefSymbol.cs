namespace ReferenceGenerator.XmlDocs;

public sealed class TypeparamrefSymbol(
    TypeparamrefSyntax syntax,
    XmlDocsSymbolReference reference)
    : XmlDocsSymbol<TypeparamrefSyntax>(syntax, XmlDocsSymbolKind.Typeparamref)
{
    public XmlDocsSymbolReference Reference => reference;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
