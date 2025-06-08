namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class SummarySymbol(
    SummarySyntax syntax,
    IReadOnlyList<IXmlDocsSymbol> children)
    : XmlDocsSymbol<SummarySyntax>(syntax, XmlDocsSymbolKind.Summary)
{
    public IReadOnlyList<IXmlDocsSymbol> Children => children;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
