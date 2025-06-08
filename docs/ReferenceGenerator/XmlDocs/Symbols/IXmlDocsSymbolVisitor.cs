namespace ReferenceGenerator.XmlDocs;

public interface IXmlDocsSymbolVisitor
{
    void Visit(DocSymbol docSymbol);
    void Visit(AssemblySymbol assemblySymbol);
    void Visit(MemberSymbol membersSymbol);
    void Visit(SummarySymbol summarySymbol);
    void Visit(BlockCodeSymbol blockCodeSymbol);
    void Visit(InlineCodeSymbol inlineCodeSymbol);
    void Visit(ParamrefSymbol paramrefSymbol);
    void Visit(ParamSymbol paramSymbol);
    void Visit(RemarksSymbol remarksSymbol);
    void Visit(ReturnsSymbol returnsSymbol);
    void Visit(SeeSymbol seeSymbol);
    void Visit(TextSymbol textSymbol);
    void Visit(TypeparamrefSymbol typeparamrefSymbol);
    void Visit(TypeparamSymbol typeparamSymbol);
}
