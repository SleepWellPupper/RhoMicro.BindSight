namespace ReferenceGenerator.XmlDocs;

using System.Text;

public sealed class CommentStringVisitor(StringBuilder builder) : IStringBuildingXmlDocsVisitor
{
    public void Visit(MemberElement member)
    {
        if (!member.Summary.Equals(SummaryElement.Empty))
            member.Summary.Accept(this);

        foreach (var typeparam in member.Typeparams)
            typeparam.Accept(this);

        if (!member.Remarks.Equals(RemarksElement.Empty))
            member.Remarks.Accept(this);

        if (!member.Example.Equals(ExampleElement.Empty))
            member.Example.Accept(this);

        foreach (var param in member.Params)
            param.Accept(this);

        if (!member.Returns.Equals(ReturnsElement.Empty))
            member.Returns.Accept(this);

        foreach (var exception in member.Exceptions)
            exception.Accept(this);
    }

    public void Visit(SummaryElement summary)
    {
        builder.AppendLine("/// <summary>").Append("/// ");
        foreach (var element in summary.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </summary>");
    }

    public void Visit(ParamElement param)
    {
        builder.AppendLine($"""/// <param name="{param.Name}">""").Append("/// ");
        foreach (var element in param.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </param>");
    }

    public void Visit(ExceptionElement exception)
    {
        builder.AppendLine($"""/// <exception cref="{exception.Cref}">""").Append("/// ");
        foreach (var element in exception.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </exception>");
    }

    public void Visit(ReturnsElement returns)
    {
        builder.AppendLine("/// <returns>").Append("/// ");
        foreach (var element in returns.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </returns>");
    }

    public void Visit(RemarksElement remarks)
    {
        builder.AppendLine("/// <remarks>").Append("/// ");
        foreach (var element in remarks.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </remarks>");
    }

    public void Visit(ExampleElement example)
    {
        builder.AppendLine("/// <example>").Append("/// ");
        foreach (var element in example.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </example>");
    }

    public void Visit(TypeparamElement typeparam)
    {
        builder.AppendLine($"""/// <param name="{typeparam.Name}">""").Append("/// ");
        foreach (var element in typeparam.Elements)
            element.Accept(this);
        builder.AppendLine().AppendLine("/// </param>");
    }

    public void Visit(TextElement text) => builder.Append(text.Value);

    public void Visit(InlineCodeElement inlineCode)
    {
        builder.Append("<c>");
        foreach (var element in inlineCode.Elements)
            element.Accept(this);
        builder.Append("</c>");
    }

    public void Visit(BlockCodeElement blockCode)
    {
        builder.Append("<code>");
        foreach (var element in blockCode.Elements)
            element.Accept(this);
        builder.Append("</code>");
    }

    public void Visit(ParaElement para)
    {
        builder.Append("<para>");
        foreach (var element in para.Elements)
            element.Accept(this);
        builder.Append("</para>");
    }

    public void Visit(SeeCrefElement seeCref)
        => builder.Append($"""</ see cref="{seeCref.Cref}">""");

    public void Visit(SeeHrefElement seeHref)
        => builder.Append($"""</ see href="{seeHref.Href}">""");

    public void Visit(SeeLangwordElement seeLangword)
        => builder.Append($"""</ see langword="{seeLangword.Langword}">""");

    public void Visit(ParamrefElement paramref)
        => builder.Append($"""</paramref name="{paramref.Name}">""");

    public void Visit(TypeparamrefElement typeparamref)
        => builder.Append($"""</param name="{typeparamref.Name}">""");

    public static IXmlDocsVisitor Create(StringBuilder builder) => new CommentStringVisitor(builder);
}
