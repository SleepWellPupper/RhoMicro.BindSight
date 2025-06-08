namespace ReferenceGenerator.XmlDocs;

public abstract class XmlDocsSymbol<TSyntax>(
    TSyntax xmlDocsSyntax,
    XmlDocsSymbolKind kind)
    : IXmlDocsSymbol<TSyntax>, IXmlDocsSymbol
    where TSyntax : XmlDocsSyntax
{
    public TSyntax Syntax { get; } = xmlDocsSyntax;
    XmlDocsSyntax IXmlDocsSymbol<XmlDocsSyntax>.Syntax => Syntax;

    public XmlDocsSymbolKind Kind { get; } = kind;

    public abstract void Accept<TVisitor>(TVisitor visitor)
        where TVisitor : IXmlDocsSymbolVisitor;
}
