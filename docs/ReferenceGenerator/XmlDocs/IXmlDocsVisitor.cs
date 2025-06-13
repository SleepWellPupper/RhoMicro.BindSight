namespace ReferenceGenerator.XmlDocs;

public interface IXmlDocsVisitor
{
    void Visit(MemberElement member);

    void Visit(SummaryElement summary);
    void Visit(ParamElement param);
    void Visit(ReturnsElement returns);
    void Visit(RemarksElement remarks);
    void Visit(ExampleElement example);
    void Visit(TypeparamElement typeparam);
    void Visit(ExceptionElement exception);

    void Visit(TextElement text);
    void Visit(InlineCodeElement inlineCode);
    void Visit(BlockCodeElement blockCode);
    void Visit(ParaElement para);
    void Visit(SeeCrefElement seeCref);
    void Visit(SeeHrefElement seeHref);
    void Visit(SeeLangwordElement seeLangword);
    void Visit(ParamrefElement paramref);
    void Visit(TypeparamrefElement typeparamref);
}
