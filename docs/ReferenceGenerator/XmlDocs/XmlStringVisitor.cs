namespace ReferenceGenerator.XmlDocs;

using System.Runtime.CompilerServices;
using System.Text;

public sealed class XmlStringVisitor(StringBuilder builder) : IStringBuildingXmlDocsVisitor
{
    private Int32 _indentationDepth;
    private readonly StringBuilder _builder = builder;
    private const Int32 IndentationSpaces = 2;

    private void AppendIndentation()
    {
        var spaceCount = _indentationDepth * IndentationSpaces;

        switch (spaceCount)
        {
            case 0:
                break;
            case < 256:
                Span<Char> indentation = stackalloc Char[spaceCount];
                indentation.Fill(' ');
                _builder.Append(indentation);
                break;
            default:
                _builder.Append(new String(' ', spaceCount));
                break;
        }
    }

    private void AppendElement<TState>(
        String name,
        Action<TState, XmlStringVisitor> appendContent,
        TState state,
        params ReadOnlySpan<(String key, String value)> attributes)
    {
        AppendIndentation();
        _builder.Append($"<{name}");

        foreach (var (key, value) in attributes)
        {
            if (value is [_, ..])
                _builder.Append($" {key}=\"{value}\"");
        }

        _builder.AppendLine(">");

        _indentationDepth++;
        appendContent.Invoke(state, this);
        _indentationDepth--;

        AppendIndentation();
        _builder.AppendLine($"</{name}>");
    }

    private void AppendElement<TState>(
        String name,
        Action<TState, XmlStringVisitor> appendContent,
        TState state,
        String attributeValue,
        [CallerArgumentExpression(nameof(attributeValue))]
        String attributeKey = null!)
        => AppendElement(name, appendContent, state,
            (attributeKey.Split('.').Last().ToLowerInvariant(), attributeValue));

    private void AppendElement(
        String name,
        params ReadOnlySpan<(String key, String value)> attributes)
    {
        AppendIndentation();
        _builder.Append($"<{name}");

        foreach (var (key, value) in attributes)
        {
            if (value is [_, ..])
                _builder.Append($" {key}=\"{value}\"");
        }

        _builder.AppendLine("/>");
    }

    private void AppendElement(
        String name,
        String attributeValue,
        [CallerArgumentExpression(nameof(attributeValue))]
        String attributeKey = null!)
        => AppendElement(name, (attributeKey.Split('.').Last().ToLowerInvariant(), attributeValue));

    public void Visit(MemberElement member)
        => AppendElement("member",
            static (member, @this) =>
            {
                if (!member.Summary.Equals(SummaryElement.Empty))
                    member.Summary.Accept(@this);

                foreach (var typeparam in member.Typeparams)
                    typeparam.Accept(@this);

                if (!member.Remarks.Equals(RemarksElement.Empty))
                    member.Remarks.Accept(@this);

                if (!member.Example.Equals(ExampleElement.Empty))
                    member.Example.Accept(@this);

                foreach (var param in member.Params)
                    param.Accept(@this);

                if (!member.Returns.Equals(ReturnsElement.Empty))
                    member.Returns.Accept(@this);

                foreach (var exception in member.Exceptions)
                    exception.Accept(@this);
            },
            member);

    public void Visit(ExceptionElement exception)
        => AppendElement(
            "exception",
            static (exception, @this) =>
            {
                foreach (var element in exception.Elements)
                    element.Accept(@this);
            },
            exception,
            exception.Cref);

    public void Visit(TextElement text)
        => AppendElement(
            "#text",
            static (text, @this) =>
            {
                @this.AppendIndentation();
                var sanitized = text.Value
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
                @this._builder.AppendLine(sanitized);
            },
            text);

    public void Visit(InlineCodeElement inlineCode)
        => AppendElement(
            "c",
            static (inlineCode, @this) =>
            {
                foreach (var element in inlineCode.Elements)
                    element.Accept(@this);
            },
            inlineCode);


    public void Visit(BlockCodeElement blockCode)
        => AppendElement(
            "code",
            static (blockCode, @this) =>
            {
                foreach (var element in blockCode.Elements)
                    element.Accept(@this);
            },
            blockCode);

    public void Visit(ParaElement para)
        => AppendElement(
            "para",
            static (para, @this) =>
            {
                foreach (var element in para.Elements)
                    element.Accept(@this);
            },
            para);

    public void Visit(SeeCrefElement seeCref)
        => AppendElement("see", seeCref.Cref);

    public void Visit(SeeHrefElement seeHref)
        => AppendElement("see", seeHref.Href);

    public void Visit(SeeLangwordElement seeLangword)
        => AppendElement("see", seeLangword.Langword);


    public void Visit(ParamrefElement paramref)
        => AppendElement("paramref", paramref.Name);

    public void Visit(TypeparamrefElement typeparamref)
        => AppendElement("typeparamref", typeparamref.Name);

    public static IXmlDocsVisitor Create(StringBuilder builder) => new XmlStringVisitor(builder);

    public void Visit(SummaryElement summary)
        => AppendElement(
            "summary",
            static (summary, @this) =>
            {
                foreach (var element in summary.Elements)
                    element.Accept(@this);
            },
            summary);

    public void Visit(ParamElement param)
        => AppendElement(
            "param",
            static (param, @this) =>
            {
                foreach (var element in param.Elements)
                    element.Accept(@this);
            },
            param,
            param.Name);

    public void Visit(ReturnsElement returns)
        => AppendElement(
            "returns",
            static (returns, @this) =>
            {
                foreach (var element in returns.Elements)
                    element.Accept(@this);
            },
            returns);

    public void Visit(RemarksElement remarks)
        => AppendElement(
            "remarks",
            static (remarks, @this) =>
            {
                foreach (var element in remarks.Elements)
                    element.Accept(@this);
            },
            remarks);

    public void Visit(ExampleElement example)
        => AppendElement(
            "example",
            static (example, @this) =>
            {
                foreach (var element in example.Elements)
                    element.Accept(@this);
            },
            example);

    public void Visit(TypeparamElement typeparam)
        => AppendElement(
            "typeparam",
            static (typeparam, @this) =>
            {
                foreach (var element in typeparam.Elements)
                    element.Accept(@this);
            },
            typeparam,
            typeparam.Name);
}
