namespace ReferenceGenerator.XmlDocs;

public abstract class TreeTraversingXmlDocsVisitor : IXmlDocsVisitor
{
    public virtual void Visit(MemberElement member)
    {
        OnBeforeVisit(member);

        member.Summary.Accept(this);

        Visit(member.Typeparams);

        member.Remarks.Accept(this);
        member.Example.Accept(this);

        Visit(member.Params);

        member.Returns.Accept(this);

        Visit(member.Exceptions);

        OnAfterVisit(member);
    }

    protected virtual void Visit(XmlDocsElements<TypeparamElement> typeparams)
    {
        OnBeforeVisit(typeparams);
        foreach (var element in typeparams)
            element.Accept(this);
        OnAfterVisit(typeparams);
    }

    protected virtual void Visit(XmlDocsElements<ParamElement> @params)
    {
        OnBeforeVisit(@params);
        foreach (var element in @params)
            element.Accept(this);
        OnAfterVisit(@params);
    }

    protected virtual void Visit(XmlDocsElements<ExceptionElement> exceptions)
    {
        OnBeforeVisit(exceptions);
        foreach (var element in exceptions)
            element.Accept(this);
        OnAfterVisit(exceptions);
    }

    protected virtual void OnBeforeVisit(XmlDocsElements<ParamElement> @params)
    {
    }

    protected virtual void OnAfterVisit(XmlDocsElements<ParamElement> @params)
    {
    }

    protected virtual void OnBeforeVisit(XmlDocsElements<TypeparamElement> typeparams)
    {
    }

    protected virtual void OnAfterVisit(XmlDocsElements<TypeparamElement> typeparams)
    {
    }

    protected virtual void OnBeforeVisit(XmlDocsElements<ExceptionElement> exceptions)
    {
    }

    protected virtual void OnAfterVisit(XmlDocsElements<ExceptionElement> exceptions)
    {
    }


    protected virtual void OnBeforeVisit(MemberElement member)
    {
    }

    protected virtual void OnAfterVisit(MemberElement member)
    {
    }

    public virtual void Visit(SummaryElement summary)
    {
        OnBeforeVisit(summary);
        foreach (var element in summary.Elements)
            element.Accept(this);
        OnAfterVisit(summary);
    }

    protected virtual void OnBeforeVisit(SummaryElement summary)
    {
    }

    protected virtual void OnAfterVisit(SummaryElement summary)
    {
    }

    public virtual void Visit(ParamElement param)
    {
        OnBeforeVisit(param);
        foreach (var element in param.Elements)
            element.Accept(this);
        OnAfterVisit(param);
    }

    protected virtual void OnBeforeVisit(ParamElement param)
    {
    }

    protected virtual void OnAfterVisit(ParamElement param)
    {
    }

    public virtual void Visit(ReturnsElement returns)
    {
        OnBeforeVisit(returns);
        foreach (var element in returns.Elements)
            element.Accept(this);
        OnAfterVisit(returns);
    }

    protected virtual void OnBeforeVisit(ReturnsElement returns)
    {
    }

    protected virtual void OnAfterVisit(ReturnsElement returns)
    {
    }

    public virtual void Visit(RemarksElement remarks)
    {
        OnBeforeVisit(remarks);
        foreach (var element in remarks.Elements)
            element.Accept(this);
        OnAfterVisit(remarks);
    }

    protected virtual void OnBeforeVisit(RemarksElement remarks)
    {
    }

    protected virtual void OnAfterVisit(RemarksElement remarks)
    {
    }

    public virtual void Visit(ExampleElement example)
    {
        OnBeforeVisit(example);
        foreach (var element in example.Elements)
            element.Accept(this);
        OnAfterVisit(example);
    }

    protected virtual void OnBeforeVisit(ExampleElement example)
    {
    }

    protected virtual void OnAfterVisit(ExampleElement example)
    {
    }

    public virtual void Visit(TypeparamElement typeparam)
    {
        OnBeforeVisit(typeparam);
        foreach (var element in typeparam.Elements)
            element.Accept(this);
        OnAfterVisit(typeparam);
    }

    protected virtual void OnBeforeVisit(TypeparamElement typeparam)
    {
    }

    protected virtual void OnAfterVisit(TypeparamElement typeparam)
    {
    }

    public virtual void Visit(ExceptionElement exception)
    {
        OnBeforeVisit(exception);
        foreach (var element in exception.Elements)
            element.Accept(this);
        OnAfterVisit(exception);
    }

    protected virtual void OnBeforeVisit(ExceptionElement exception)
    {
    }

    protected virtual void OnAfterVisit(ExceptionElement exception)
    {
    }

    public virtual void Visit(TextElement text)
    {
        OnBeforeVisit(text);
        OnAfterVisit(text);
    }

    protected virtual void OnBeforeVisit(TextElement text)
    {
    }

    protected virtual void OnAfterVisit(TextElement text)
    {
    }

    public virtual void Visit(InlineCodeElement inlineCode)
    {
        OnBeforeVisit(inlineCode);
        foreach (var element in inlineCode.Elements)
            element.Accept(this);
        OnAfterVisit(inlineCode);
    }

    protected virtual void OnBeforeVisit(InlineCodeElement inlineCode)
    {
    }

    protected virtual void OnAfterVisit(InlineCodeElement inlineCode)
    {
    }

    public virtual void Visit(BlockCodeElement blockCode)
    {
        OnBeforeVisit(blockCode);
        foreach (var element in blockCode.Elements)
            element.Accept(this);
        OnAfterVisit(blockCode);
    }

    protected virtual void OnBeforeVisit(BlockCodeElement blockCode)
    {
    }

    protected virtual void OnAfterVisit(BlockCodeElement blockCode)
    {
    }

    public virtual void Visit(ParaElement para)
    {
        OnBeforeVisit(para);
        foreach (var element in para.Elements)
            element.Accept(this);
        OnAfterVisit(para);
    }

    protected virtual void OnBeforeVisit(ParaElement para)
    {
    }

    protected virtual void OnAfterVisit(ParaElement para)
    {
    }

    public virtual void Visit(SeeCrefElement seeCref)
    {
        OnBeforeVisit(seeCref);
        OnAfterVisit(seeCref);
    }

    protected virtual void OnBeforeVisit(SeeCrefElement seeCref)
    {
    }

    protected virtual void OnAfterVisit(SeeCrefElement seeCref)
    {
    }

    public virtual void Visit(SeeHrefElement seeHref)
    {
        OnBeforeVisit(seeHref);
        OnAfterVisit(seeHref);
    }

    protected virtual void OnBeforeVisit(SeeHrefElement seeHref)
    {
    }

    protected virtual void OnAfterVisit(SeeHrefElement seeHref)
    {
    }

    public virtual void Visit(SeeLangwordElement seeLangword)
    {
        OnBeforeVisit(seeLangword);
        OnAfterVisit(seeLangword);
    }

    protected virtual void OnBeforeVisit(SeeLangwordElement seeLangword)
    {
    }

    protected virtual void OnAfterVisit(SeeLangwordElement seeLangword)
    {
    }

    public virtual void Visit(ParamrefElement paramref)
    {
        OnBeforeVisit(paramref);
        OnAfterVisit(paramref);
    }

    protected virtual void OnBeforeVisit(ParamrefElement paramref)
    {
    }

    protected virtual void OnAfterVisit(ParamrefElement paramref)
    {
    }

    public virtual void Visit(TypeparamrefElement typeparamref)
    {
        OnBeforeVisit(typeparamref);
        OnAfterVisit(typeparamref);
    }

    protected virtual void OnBeforeVisit(TypeparamrefElement typeparamref)
    {
    }

    protected virtual void OnAfterVisit(TypeparamrefElement typeparamref)
    {
    }
}
