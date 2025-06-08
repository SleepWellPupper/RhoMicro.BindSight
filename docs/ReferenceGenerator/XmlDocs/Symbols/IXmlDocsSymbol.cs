namespace ReferenceGenerator.XmlDocs;

public interface IXmlDocsSymbol<out TSyntax>
    where TSyntax : XmlDocsSyntax
{
    TSyntax Syntax { get; }
    XmlDocsSymbolKind Kind { get; }

    void Accept<TVisitor>(TVisitor visitor)
        where TVisitor : IXmlDocsSymbolVisitor;
}

public interface IXmlDocsSymbol : IXmlDocsSymbol<XmlDocsSyntax>;
