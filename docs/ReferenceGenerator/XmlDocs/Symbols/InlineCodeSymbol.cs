namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class InlineCodeSymbol(
    InlineCodeSyntax xmlDocsSyntax,
    ImmutableArray<IXmlDocsSymbol> children)
    : XmlDocsSymbol<InlineCodeSyntax>(xmlDocsSyntax, XmlDocsSymbolKind.InlineCode)
{
    public ImmutableArray<IXmlDocsSymbol> Children { get; } = children;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
