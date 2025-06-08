namespace ReferenceGenerator.XmlDocs;

using System.Runtime.CompilerServices;
using System.Text;

public sealed class XmlStringVisitor(StringBuilder builder) : IXmlDocsSyntaxVisitor
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

    public void Visit(DocSyntax docSyntax)
        => AppendElement("doc",
            static (docSyntax, @this) =>
            {
                docSyntax.Assembly.Accept(@this);
                docSyntax.Members.Accept(@this);
            },
            docSyntax);

    public void Visit(AssemblySyntax assemblySyntax)
        => AppendElement("assembly",
            static (assemblySyntax, @this) => assemblySyntax.Name.Accept(@this),
            assemblySyntax);

    public void Visit(NameSyntax nameSyntax)
        => AppendElement("name",
            static (assemblyNameSyntax, @this) => assemblyNameSyntax.Text.Accept(@this),
            nameSyntax);

    public void Visit(MembersSyntax membersSyntax) => throw new NotImplementedException();

    public void Visit(MemberSyntax memberSyntax)
        => AppendElement("member",
            static (memberSyntax, @this) =>
            {
                foreach (var element in memberSyntax.Elements)
                    element.Accept(@this);

                memberSyntax.Inheritdoc?.Accept(@this);
            },
            memberSyntax,
            ("name", memberSyntax.Name));

    public void Visit(MainElementSyntax mainElementSyntax) => mainElementSyntax.Child.Accept(this);

    public void Visit(NestedElementsOrInheritdocSyntax nestedElementsOrInheritdocSyntax)
        => nestedElementsOrInheritdocSyntax.Child.Accept(this);

    public void Visit(NestedElementsSyntax nestedElementsSyntax)
    {
        foreach (var child in nestedElementsSyntax.Children)
            child.Accept(this);
    }

    public void Visit(InheritdocSyntax inheritdocSyntax) => AppendElement("inheritdoc", inheritdocSyntax.Cref);

    public void Visit(TextSyntax textSyntax)
        => AppendElement(
            "#text",
            static (textSyntax, @this) =>
            {
                @this.AppendIndentation();
                var sanitized = textSyntax.Text
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
                @this._builder.AppendLine(sanitized);
            },
            textSyntax);

    public void Visit(InlineCodeSyntax inlineCodeSyntax)
        => AppendElement(
            "c",
            static (inlineCodeSyntax, @this) => inlineCodeSyntax.NestedElements.Accept(@this),
            inlineCodeSyntax);

    public void Visit(BlockCodeSyntax blockCodeSyntax)
        => AppendElement(
            "code",
            static (blockCodeSyntax, @this) => blockCodeSyntax.NestedElements.Accept(@this),
            blockCodeSyntax);

    public void Visit(SeeSyntax seeSyntax)
        => AppendElement(
            "see",
            ("cref", seeSyntax.Cref),
            ("href", seeSyntax.Href),
            ("langword", seeSyntax.Langword));

    public void Visit(ParamrefSyntax paramrefSyntax) => AppendElement("paramref", paramrefSyntax.Name);

    public void Visit(TypeparamrefSyntax typeparamrefSyntax) => AppendElement("typeparamref", typeparamrefSyntax.Name);

    public void Visit(NestedElementSyntax nestedElementSyntax) => nestedElementSyntax.Child.Accept(this);

    public void Visit(SummarySyntax summarySyntax)
        => AppendElement(
            "summary",
            static (summarySyntax, @this) => summarySyntax.Child.Accept(@this),
            summarySyntax);

    public void Visit(ParamSyntax paramSyntax)
        => AppendElement(
            "param",
            static (paramSyntax, @this) => paramSyntax.Child.Accept(@this),
            paramSyntax,
            paramSyntax.Name);

    public void Visit(ReturnsSyntax returnsSyntax)
        => AppendElement(
            "returns",
            static (returnsSyntax, @this) => returnsSyntax.Child.Accept(@this),
            returnsSyntax);

    public void Visit(RemarksSyntax remarksSyntax)
        => AppendElement(
            "remarks",
            static (remarksSyntax, @this) => remarksSyntax.Child.Accept(@this),
            remarksSyntax);

    public void Visit(TypeparamSyntax typeparamSyntax)
        => AppendElement(
            "typeparam",
            static (typeparamSyntax, @this) => typeparamSyntax.Child.Accept(@this),
            typeparamSyntax,
            typeparamSyntax.Name);
}
