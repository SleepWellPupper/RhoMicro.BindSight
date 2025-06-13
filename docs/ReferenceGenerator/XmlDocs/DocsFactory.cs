namespace ReferenceGenerator.XmlDocs;

public static class DocsFactory
{
    public static MemberElement Member(String summary)
        => Member(Summary(summary));

    public static MemberElement Member(params XmlDocsElement[] elements)
    {
        if (elements is [])
            return MemberElement.Empty;

        var summary = SummaryElement.Empty;
        var remarks = RemarksElement.Empty;
        var returns = ReturnsElement.Empty;
        var example = ExampleElement.Empty;
        var @params = new HashSet<ParamElement>(XmlDocsElementTypeComparer.Instance);
        var typeparams = new HashSet<TypeparamElement>(XmlDocsElementTypeComparer.Instance);
        var exceptions = new HashSet<ExceptionElement>(XmlDocsElementTypeComparer.Instance);

        foreach (var element in elements)
        {
            switch (element.Kind)
            {
                case XmlDocsKind.Summary:
                    summary = (SummaryElement)element;
                    break;
                case XmlDocsKind.Remarks:
                    remarks = (RemarksElement)element;
                    break;
                case XmlDocsKind.Returns:
                    returns = (ReturnsElement)element;
                    break;
                case XmlDocsKind.Example:
                    example = (ExampleElement)element;
                    break;
                case XmlDocsKind.Param:
                    @params.Add((ParamElement)element);
                    break;
                case XmlDocsKind.Typeparam:
                    typeparams.Add((TypeparamElement)element);
                    break;
                case XmlDocsKind.Exception:
                    exceptions.Add((ExceptionElement)element);
                    break;
            }
        }

        var result = new MemberElement(
            summary,
            returns,
            example,
            remarks,
            [..@params],
            [..typeparams],
            [..exceptions]);

        return result;
    }

    public static SummaryElement Summary(String text)
        => text is []
            ? SummaryElement.Empty
            : Summary(Text(text));

    public static SummaryElement Summary(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? SummaryElement.Empty
            : new SummaryElement([..elements]);

    public static ReturnsElement Returns(String text)
        => text is []
            ? ReturnsElement.Empty
            : Returns(Text(text));

    public static ReturnsElement Returns(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? ReturnsElement.Empty
            : new ReturnsElement([..elements]);

    public static RemarksElement Remarks(String text)
        => text is []
            ? RemarksElement.Empty
            : Remarks(Text(text));

    public static RemarksElement Remarks(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? RemarksElement.Empty
            : new RemarksElement([..elements]);

    public static ExampleElement Example(String text)
        => text is []
            ? ExampleElement.Empty
            : Example(Text(text));

    public static ExampleElement Example(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? ExampleElement.Empty
            : new ExampleElement([..elements]);

    public static ParamElement Param(String name, String text)
        => Param(name, Text(text));

    public static ParamElement Param(String name, params ReadOnlySpan<XmlDocsChildElement> elements)
        => new ParamElement(name, [..elements]);

    public static ExceptionElement Exception(String cref, String text)
        => Exception(cref, Text(text));

    public static ExceptionElement Exception(String cref, params ReadOnlySpan<XmlDocsChildElement> elements)
        => new ExceptionElement(cref, [..elements]);

    public static TypeparamElement Typeparam(String name, String text)
        => Typeparam(name, Text(text));

    public static TypeparamElement Typeparam(String name, params ReadOnlySpan<XmlDocsChildElement> elements)
        => new TypeparamElement(name, [..elements]);

    public static TextElement Text(String value)
        => value is []
            ? TextElement.Empty
            : new TextElement(value);

    public static InlineCodeElement InlineCode(String text)
        => text is []
            ? InlineCodeElement.Empty
            : InlineCode(Text(text));

    public static InlineCodeElement InlineCode(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? InlineCodeElement.Empty
            : new InlineCodeElement([..elements]);

    public static BlockCodeElement BlockCode(String text)
        => text is []
            ? BlockCodeElement.Empty
            : BlockCode(Text(text));

    public static ParaElement Para(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? ParaElement.Empty
            : new ParaElement([..elements]);

    public static ParaElement Para(String text)
        => text is []
            ? ParaElement.Empty
            : Para(Text(text));

    public static BlockCodeElement BlockCode(params ReadOnlySpan<XmlDocsChildElement> elements)
        => elements is []
            ? BlockCodeElement.Empty
            : new BlockCodeElement([..elements]);

    public static ParamrefElement Paramref(String name)
        => new ParamrefElement(name);

    public static TypeparamrefElement Typeparamref(String name)
        => new TypeparamrefElement(name);

    public static SeeCrefElement SeeCref(String cref)
        => new SeeCrefElement(cref);

    public static SeeHrefElement SeeHref(String href)
        => new SeeHrefElement(href);

    public static SeeLangwordElement SeeLangword(String langword)
        => new SeeLangwordElement(langword);
}
