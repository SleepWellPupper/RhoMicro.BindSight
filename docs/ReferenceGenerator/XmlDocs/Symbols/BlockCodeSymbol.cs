namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class BlockCodeSymbol(
    BlockCodeSyntax xmlDocsSyntax,
    ImmutableArray<IXmlDocsSymbol> children)
    : XmlDocsSymbol<BlockCodeSyntax>(xmlDocsSyntax, XmlDocsSymbolKind.BlockCode)
{
    public ImmutableArray<IXmlDocsSymbol> Children { get; } = children;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
