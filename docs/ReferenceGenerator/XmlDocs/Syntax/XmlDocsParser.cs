namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Xml;

public sealed class XmlDocsParser
{
    private XmlDocsParser(XmlDocument? document, XmlException? exception, XmlDocsParserOptions options)
    {
        Debug.Assert((document, exception) is not (null, null));

        _document = document;
        _exception = exception;
        _options = options;
    }

    public XmlDocsParser(XmlDocument document, XmlDocsParserOptions options)
        : this(document, null, options)
    {
    }

    private XmlDocsParser(XmlException exception, XmlDocsParserOptions options)
        : this(null, exception, options)
    {
    }

    private readonly List<XmlDocsDiagnostic> _errors = [];
    private readonly List<XmlDocsDiagnostic> _warnings = [];
    private readonly XmlDocument? _document;
    private readonly XmlException? _exception;
    private readonly XmlDocsParserOptions _options;

    [MemberNotNullWhen(true, nameof(_document)),
     MemberNotNullWhen(false, nameof(_exception))]
    private Boolean HasDocument => _document is not null;

    [MemberNotNullWhen(true, nameof(_exception)),
     MemberNotNullWhen(false, nameof(_document))]
    private Boolean HasXmlException => _exception is not null;

    public static XmlDocsParser Create(String source) => Create(source, XmlDocsParserOptions.Default);

    public static XmlDocsParser Create(String source, XmlDocsParserOptions options)
    {
        try
        {
            var document = new XmlDocument();
            document.LoadXml(source);
            var result = new XmlDocsParser(document, options);

            return result;
        }
        catch (XmlException exception)
        {
            var result = new XmlDocsParser(exception, options);

            return result;
        }
    }

    public ParseResult Parse(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        _errors.Clear();
        _warnings.Clear();

        var result = HasXmlException
            ? GetXmlErrorResult()
            : GetSuccessOrErrorResult(ct);

        return result;
    }

    private ParseResult GetSuccessOrErrorResult(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (Doc(out var doc, ct))
            return new ParseResult.Success.DocResult(doc, _warnings);

        if (Member(out var member, ct))
            return new ParseResult.Success.MemberResult(member, _warnings);

        return new ParseResult.Failure.ParseFailure(_errors, _warnings);
    }

    private ParseResult.Failure.XmlFailure GetXmlErrorResult()
    {
        if (_options.ThrowXmlExceptions)
            ThrowXmlError();

        Debug.Assert(HasXmlException);

        return new ParseResult.Failure.XmlFailure(_exception);
    }

    private void ThrowXmlError() =>
        throw new InvalidOperationException(
            $"Unable to parse documentation: an xml parsing error occurred while preparing the source.",
            _exception);

    private Boolean EmitError(DiagnosticKinds kind, params ReadOnlySpan<Object?> arguments)
    {
        Debug.Assert(kind.GetPopCount() is 1);

        var isError = _options.SupportedErrors.HasFlag(kind);

        if (isError)
        {
            var error = XmlDocsDiagnostic.Create(kind, _options.ErrorFormatProvider, arguments);
            _errors.Add(error);
        }
        else if (_options.SupportedWarnings.HasFlag(kind))
        {
            var warning = XmlDocsDiagnostic.Create(kind, _options.ErrorFormatProvider, arguments);
            _warnings.Add(warning);
        }

        return isError;
    }

    private Boolean Doc([NotNullWhen(true)] out DocSyntax? doc, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Debug.Assert(HasDocument);

        doc = null;

        var xmlRoot = _document.DocumentElement;
        if (xmlRoot is null)
            return false;

        if (xmlRoot.Name is not "doc")
            return false;

        var xmlChildren = xmlRoot.ChildNodes.OfType<XmlNode>();

        AssemblySyntax? assembly = null;
        MembersSyntax? members = null;

        foreach (var xmlChild in xmlChildren)
        {
            ct.ThrowIfCancellationRequested();

            if (Assembly(xmlChild, out var a, ct))
            {
                assembly = a;
                continue;
            }

            if (Members(xmlChild, out var m, ct))
            {
                members = m;
                continue;
            }

            return false;
        }

        if (assembly is null || members is null)
            return false;

        doc = new DocSyntax(assembly, members);
        return true;
    }

    private Boolean Members(XmlNode xmlNode, [NotNullWhen(true)] out MembersSyntax? members, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        members = null;

        if (xmlNode.Name is not "members")
            return false;

        var children = new List<MemberSyntax>();
        foreach (var xmlChild in xmlNode.ChildNodes.OfType<XmlNode>())
        {
            ct.ThrowIfCancellationRequested();

            if (!Member(xmlChild, out var child, ct))
                return false;

            children.Add(child);
        }

        members = new MembersSyntax(new SyntaxChildren<MemberSyntax>(children));
        return true;
    }

    private Boolean Assembly(
        XmlNode xmlNode,
        [NotNullWhen(true)] out AssemblySyntax? assembly,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        assembly = null;

        if (xmlNode.Name is not "assembly")
            return false;

        var xmlChildren = xmlNode.ChildNodes.OfType<XmlNode>();
        NameSyntax? assemblyName = null;

        foreach (var xmlChild in xmlChildren)
        {
            if (Name(xmlChild, out var n, ct))
                assemblyName = n;
        }

        if (assemblyName is null)
            return false;

        assembly = new AssemblySyntax(assemblyName);
        return true;
    }

    private Boolean Name(
        XmlNode xmlNode,
        [NotNullWhen(true)] out NameSyntax? name,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        name = null;

        if (xmlNode.Name is not "name")
            return false;

        if (xmlNode.ChildNodes is not [{ } child])
            return false;

        if (!Text(child, out var text, ct))
            return false;

        name = new NameSyntax(text);
        return true;
    }

    private Boolean Member([NotNullWhen(true)] out MemberSyntax? member, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Debug.Assert(HasDocument);

        member = null;

        var xmlRoot = _document.DocumentElement;
        if (xmlRoot is null)
            return false;

        if (!Member(xmlRoot, out var m, ct))
            return false;

        member = m;
        return true;
    }

    private Boolean Member(XmlNode xmlNode, [NotNullWhen(true)] out MemberSyntax? member, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        member = null;

        if (xmlNode.Name is not "member")
            return false;

        if (!Attribute(xmlNode, "name", out var name, ct))
            return false;

        if (xmlNode.ChildNodes.Count is 0)
            return false;

        InheritdocSyntax? inheritdoc = null;
        var mainElements = new List<MainElementSyntax>();
        foreach (var child in xmlNode.ChildNodes.OfType<XmlNode>())
        {
            if (Inheritdoc(child, out var i, ct))
            {
                inheritdoc ??= i;
            }
            else
            {
                if (!MainElement(child, out var mainElement, ct))
                {
                    if (EmitError(DiagnosticKinds.UnrecognizedMainElement, child.OuterXml))
                        return false;

                    continue;
                }

                mainElements.Add(mainElement);
            }
        }

        member = new MemberSyntax(
            name,
            inheritdoc,
            new SyntaxChildren<MainElementSyntax>(mainElements));
        return true;
    }

    private String Attribute(
        XmlNode xmlNode,
        String name,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = Attribute(xmlNode, name, out var value, ct)
            ? value
            : String.Empty;

        return result;
    }

    private Boolean Attribute(
        XmlNode xmlNode,
        String name,
        [NotNullWhen(true)] out String? value,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        value = null;

        if (xmlNode.Attributes is null)
            return false;

        foreach (var attribute in xmlNode.Attributes.OfType<XmlAttribute>())
        {
            ct.ThrowIfCancellationRequested();

            if (attribute.Name != name)
                continue;

            value = attribute.Value;
            return true;
        }

        return false;
    }

    private Boolean NonEmptyAttribute(
        XmlNode xmlNode,
        String name,
        [NotNullWhen(true)] out String? value,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        value = null;

        if (!Attribute(xmlNode, name, out var possiblyEmptyValue, ct))
            return false;

        if (possiblyEmptyValue is [])
            return false;

        value = possiblyEmptyValue;
        return true;
    }

    private Boolean MainElement(
        XmlNode xmlNode,
        [NotNullWhen(true)] out MainElementSyntax? mainElement,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (Summary(xmlNode, out var summary, ct))
        {
            mainElement = new MainElementSyntax(summary);
            return true;
        }

        if (Returns(xmlNode, out var returns, ct))
        {
            mainElement = new MainElementSyntax(returns);
            return true;
        }

        if (Param(xmlNode, out var param, ct))
        {
            mainElement = new MainElementSyntax(param);
            return true;
        }

        if (TypeParam(xmlNode, out var typeParam, ct))
        {
            mainElement = new MainElementSyntax(typeParam);
            return true;
        }

        if (Inheritdoc(xmlNode, out var inheritDoc, ct))
        {
            mainElement = new MainElementSyntax(inheritDoc);
            return true;
        }

        if (Remarks(xmlNode, out var remarks, ct))
        {
            mainElement = new MainElementSyntax(remarks);
            return true;
        }

        mainElement = null;
        return false;
    }

    private Boolean Remarks(
        XmlNode xmlNode,
        [NotNullWhen(true)] out RemarksSyntax? remarks,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        remarks = null;

        if (xmlNode.Name is not "remarks")
            return false;

        if (!NestedElementsOrInheritdoc(xmlNode, out var child, ct))
            return false;

        remarks = new RemarksSyntax(child);
        return true;
    }

    private Boolean Inheritdoc(
        XmlNode xmlNode,
        [NotNullWhen(true)] out InheritdocSyntax? inheritdoc,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        inheritdoc = null;

        if (xmlNode.Name is not "inheritdoc")
            return false;

        var cref = Attribute(xmlNode, "cref", ct);
        inheritdoc = new InheritdocSyntax(cref);

        return true;
    }

    private Boolean Summary(
        XmlNode xmlNode,
        [NotNullWhen(true)] out SummarySyntax? summary,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        summary = null;

        if (xmlNode.Name is not "summary")
            return false;

        if (!NestedElementsOrInheritdoc(xmlNode, out var child, ct))
            return false;

        summary = new SummarySyntax(child);
        return true;
    }

    private Boolean Returns(
        XmlNode xmlNode,
        [NotNullWhen(true)] out ReturnsSyntax? returns,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        returns = null;

        if (xmlNode.Name is not "returns")
            return false;

        if (!NestedElementsOrInheritdoc(xmlNode, out var child, ct))
            return false;

        returns = new ReturnsSyntax(child);
        return true;
    }

    private Boolean Param(
        XmlNode xmlNode,
        [NotNullWhen(true)] out ParamSyntax? param,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        param = null;

        if (xmlNode.Name is not "param")
            return false;

        if (!NonEmptyAttribute(xmlNode, "name", out var name, ct))
            return false;

        if (!NestedElementsOrInheritdoc(xmlNode, out var child, ct))
            return false;

        param = new ParamSyntax(name, child);
        return true;
    }

    private Boolean TypeParam(
        XmlNode xmlNode,
        [NotNullWhen(true)] out TypeparamSyntax? param,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        param = null;

        if (xmlNode.Name is not "typeparam")
            return false;

        if (!NonEmptyAttribute(xmlNode, "name", out var name, ct))
            return false;

        if (!NestedElementsOrInheritdoc(xmlNode, out var child, ct))
            return false;

        param = new TypeparamSyntax(name, child);
        return true;
    }

    private Boolean NestedElementsOrInheritdoc(
        XmlNode xmlNode,
        [NotNullWhen(true)] out NestedElementsOrInheritdocSyntax? nestedElementsOrInheritdoc,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        nestedElementsOrInheritdoc = null;

        if (xmlNode.ChildNodes is [{ } singleXmlChild])
        {
            if (Inheritdoc(singleXmlChild, out var inheritdoc, ct))
            {
                nestedElementsOrInheritdoc = new NestedElementsOrInheritdocSyntax(inheritdoc);
                return true;
            }
        }

        if (!NestedElements(xmlNode, out var nestedElements, ct))
            return false;

        nestedElementsOrInheritdoc = new NestedElementsOrInheritdocSyntax(nestedElements);

        return true;
    }

    private Boolean NestedElements(
        XmlNode xmlNode,
        [NotNullWhen(true)] out NestedElementsSyntax? nestedElements,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        nestedElements = null;

        var xmlChildren = xmlNode.ChildNodes.OfType<XmlNode>();
        var children = new List<NestedElementSyntax>();

        foreach (var xmlChild in xmlChildren)
        {
            ct.ThrowIfCancellationRequested();

            if (!NestedElement(xmlChild, out var nestedElement, ct))
            {
                if (EmitError(DiagnosticKinds.UnrecognizedNestedElement, xmlChild.OuterXml))
                    return false;

                continue;
            }

            children.Add(nestedElement);
        }

        nestedElements = new NestedElementsSyntax(new SyntaxChildren<NestedElementSyntax>(children));
        return true;
    }

    private Boolean NestedElement(
        XmlNode xmlNode,
        [NotNullWhen(true)] out NestedElementSyntax? nestedElement,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (Text(xmlNode, out var text, ct))
        {
            nestedElement = new NestedElementSyntax(text);
            return true;
        }

        if (InlineCode(xmlNode, out var inlineCode, ct))
        {
            nestedElement = new NestedElementSyntax(inlineCode);
            return true;
        }

        if (BlockCode(xmlNode, out var blockCode, ct))
        {
            nestedElement = new NestedElementSyntax(blockCode);
            return true;
        }

        if (See(xmlNode, out var see, ct))
        {
            nestedElement = new NestedElementSyntax(see);
            return true;
        }

        if (Paramref(xmlNode, out var paramref, ct))
        {
            nestedElement = new NestedElementSyntax(paramref);
            return true;
        }

        if (Typeparamref(xmlNode, out var typeparamref, ct))
        {
            nestedElement = new NestedElementSyntax(typeparamref);
            return true;
        }

        nestedElement = null;
        return false;
    }

    private Boolean Typeparamref(
        XmlNode xmlNode,
        [NotNullWhen(true)] out TypeparamrefSyntax? typeparamref,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        typeparamref = null;

        if (xmlNode.Name is not "typeparamref")
            return false;

        if (!NonEmptyAttribute(xmlNode, "name", out var name, ct))
            return false;

        typeparamref = new TypeparamrefSyntax(name);
        return true;
    }

    private Boolean Paramref(
        XmlNode xmlNode,
        [NotNullWhen(true)] out ParamrefSyntax? paramref,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        paramref = null;

        if (xmlNode.Name is not "paramref")
            return false;

        if (!NonEmptyAttribute(xmlNode, "name", out var name, ct))
            return false;

        paramref = new ParamrefSyntax(name);
        return true;
    }

    private Boolean See(
        XmlNode xmlNode,
        [NotNullWhen(true)] out SeeSyntax? see,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        see = null;

        if (xmlNode.Name is not "see")
            return false;

        var cref = Attribute(xmlNode, "cref", ct);
        var href = Attribute(xmlNode, "href", ct);
        var langword = Attribute(xmlNode, "langword", ct);

        if (cref is [] && href is [] && langword is [])
            return false;

        see = new SeeSyntax(
            Cref: cref,
            Href: href,
            Langword: langword);
        return true;
    }

    private Boolean Text(
        XmlNode xmlNode,
        [NotNullWhen(true)] out TextSyntax? text,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        text = null;

        if (xmlNode.Name is not "#text")
            return false;

        var value = xmlNode.InnerText;
        text = new TextSyntax(value);

        return true;
    }

    private Boolean InlineCode(
        XmlNode xmlNode,
        [NotNullWhen(true)] out InlineCodeSyntax? inlineCode,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        inlineCode = null;

        if (xmlNode.Name is not "c")
            return false;

        if (!NestedElements(xmlNode, out var nestedElements, ct))
            return false;

        inlineCode = new InlineCodeSyntax(nestedElements);
        return true;
    }

    private Boolean BlockCode(
        XmlNode xmlNode,
        [NotNullWhen(true)] out BlockCodeSyntax? blockCode,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        blockCode = null;

        if (xmlNode.Name is not "code")
            return false;

        if (!NestedElements(xmlNode, out var nestedElements, ct))
            return false;

        blockCode = new BlockCodeSyntax(nestedElements);
        return true;
    }
}
