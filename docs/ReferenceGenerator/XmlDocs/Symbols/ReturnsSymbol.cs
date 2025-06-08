namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class ReturnsSymbol(
    ReturnsSyntax syntax,
    IReadOnlyList<IXmlDocsSymbol> children)
    : XmlDocsSymbol<ReturnsSyntax>(syntax, XmlDocsSymbolKind.Returns)
{
    public IReadOnlyList<IXmlDocsSymbol> Children => children;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
