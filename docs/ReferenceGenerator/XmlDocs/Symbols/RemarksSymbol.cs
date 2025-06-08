namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class RemarksSymbol(
    RemarksSyntax syntax,
    IReadOnlyList<IXmlDocsSymbol> children)
    : XmlDocsSymbol<RemarksSyntax>(syntax, XmlDocsSymbolKind.Remarks)
{
    IReadOnlyList<IXmlDocsSymbol> Children => children;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
