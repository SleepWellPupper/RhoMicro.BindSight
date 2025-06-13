namespace ReferenceGenerator.XmlDocs;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Runtime.InteropServices.JavaScript;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public sealed class XmlDocsProvider
{
    private XmlDocsProvider(Digraph<XmlDocsElement> elements)
    {
        _elements = elements;
    }

    private readonly ConcurrentDictionary<String, MemberElement> _members = [];
    private readonly Digraph<XmlDocsElement> _elements;

    public static XmlDocsProvider Create(
        XmlDocsSemanticModel semanticModel,
        CancellationToken ct)
        => Create(semanticModel, XmlDocsProviderOptions.Default, ct);

    public static XmlDocsProvider Create(
        XmlDocsSemanticModel semanticModel,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var elementsGraphBuilder = new DigraphBuilder<XmlDocsElement>(XmlDocsElementTypeComparer.Instance);
        // mapping type->member->inherited_docs_id
        var interfaceImplementationsOrOverriddenMembers =
            new Dictionary<ISymbol, Dictionary<ISymbol, HashSet<String>>>(SymbolEqualityComparer.Default);

        foreach (var knownSymbol in semanticModel.KnownSymbols)
        {
            ct.ThrowIfCancellationRequested();

            if (knownSymbol is INamedTypeSymbol type)
                AddInterfaceImplementationOrOverriddenMembers(
                    type,
                    interfaceImplementationsOrOverriddenMembers,
                    ct);

            if (knownSymbol.GetDocumentationCommentId() is not { } id)
                continue;

            if (!TryGetMemberElements(knownSymbol, out var xmlElements, options, ct))
                continue;

            var references = ImmutableHashSet.CreateBuilder<String>();

            var children = new List<XmlDocsElement>();
            foreach (var xmlElement in xmlElements)
            {
                ct.ThrowIfCancellationRequested();
                if (TryParseInheritdoc(
                        knownSymbol,
                        xmlElement,
                        references,
                        interfaceImplementationsOrOverriddenMembers,
                        ct))
                    continue;
                if (TryParseSummary(xmlElement, children, options, ct))
                    continue;
                if (TryParseRemarks(xmlElement, children, options, ct))
                    continue;
                if (TryParseReturns(xmlElement, children, options, ct))
                    continue;
                if (TryParseExample(xmlElement, children, options, ct))
                    continue;
                if (TryParseParam(xmlElement, children, options, ct))
                    continue;
                if (TryParseTypeparam(xmlElement, children, options, ct))
                    continue;
                if (TryParseException(xmlElement, children, options, ct))
                    continue;
            }

            elementsGraphBuilder.Add(id, children, references);
        }

        var elementsGraph = elementsGraphBuilder.Build(ct);
        var result = new XmlDocsProvider(elementsGraph);

        return result;
    }

    /// <summary>
    /// Gets the reference id for a docs element on a member with <paramref name="id"/>.
    /// </summary>
    /// <example>
    /// Foo.Bar$param$name
    /// Foo.Bar$summary$
    /// Foo.Bar$typeparam$T
    /// </example>
    /// <returns></returns>
    private static String GetElementReferenceId(String id, String elementId)
        => $"{id}${elementId}";

    private static String GetElementId(XElement xmlElement)
        => $"{xmlElement.Name.LocalName}${xmlElement.Attribute(XName.Get("name"))?.Value}";

    private static Boolean TryParseSummary(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "summary" })
            return false;

        var children = ParseChildren(candidate, options, ct);
        var element = new SummaryElement(children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseRemarks(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "remarks" })
            return false;

        var children = ParseChildren(candidate, options, ct);
        var element = new RemarksElement(children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseReturns(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "returns" })
            return false;

        var children = ParseChildren(candidate, options, ct);
        var element = new ReturnsElement(children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseExample(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "example" })
            return false;

        var children = ParseChildren(candidate, options, ct);
        var element = new ExampleElement(children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseException(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "exception" })
            return false;

        if (candidate.Attribute(XName.Get("cref")) is not { Value: { } cref })
            return true;

        var children = ParseChildren(candidate, options, ct);
        var element = new ExceptionElement(cref, children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseParam(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "param" })
            return false;

        if (candidate.Attribute(XName.Get("name")) is not { Value: { } name })
            return true;

        var children = ParseChildren(candidate, options, ct);
        var element = new ParamElement(name, children);
        memberChildren.Add(element);
        return true;
    }

    private static Boolean TryParseTypeparam(
        XElement candidate,
        List<XmlDocsElement> memberChildren,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not { Name.LocalName: "typeparam" })
            return false;

        if (candidate.Attribute(XName.Get("name")) is not { Value: { } name })
            return true;

        var children = ParseChildren(candidate, options, ct);
        var element = new TypeparamElement(name, children);
        memberChildren.Add(element);
        return true;
    }

    private static XmlDocsElements<XmlDocsChildElement> ParseChildren(
        XElement container,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var elementsMutable = new List<XmlDocsChildElement>();

        foreach (var xmlElement in container.Nodes())
        {
            ct.ThrowIfCancellationRequested();

            if (TryParseText(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseInlineCode(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseBlockCode(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParsePara(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseParamref(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseTypeparamref(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseSeeCref(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseSeeHref(xmlElement, elementsMutable, options, ct))
                continue;
            if (TryParseSeeLangword(xmlElement, elementsMutable, options, ct))
                continue;
        }

        var result = elementsMutable.Count > 0
            ? new XmlDocsElements<XmlDocsChildElement>(elementsMutable)
            : XmlDocsElements<XmlDocsChildElement>.Empty;

        return result;
    }

    private static Boolean TryParseText(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XText textNode)
            return false;

        var text = RemoveWhitespace(textNode.Value, options) is [_, ..] value
            ? new TextElement(value)
            : TextElement.Empty;
        elements.Add(text);
        return true;
    }

    private static Boolean TryParseInlineCode(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "c" } element)
            return false;

        var children = ParseChildren(element, options, ct);
        var inlineCode = new InlineCodeElement(children);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseBlockCode(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "code" } element)
            return false;

        var children = ParseChildren(element, options, ct);
        var inlineCode = new BlockCodeElement(children);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParsePara(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "para" } element)
            return false;

        var children = ParseChildren(element, options, ct);
        var inlineCode = new ParaElement(children);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseParamref(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "paramref" } element)
            return false;

        if (element.Attribute(XName.Get("name")) is not { Value: [_, ..] name })
            return true;

        var inlineCode = new ParamrefElement(name);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseTypeparamref(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "typeparamref" } element)
            return false;

        if (element.Attribute(XName.Get("name")) is not { Value: [_, ..] name })
            return true;

        var inlineCode = new TypeparamrefElement(name);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseSeeCref(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "see" } element)
            return false;

        if (element.Attribute(XName.Get("cref")) is not { Value: [_, ..] cref })
            return false;

        var inlineCode = new SeeCrefElement(cref);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseSeeHref(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "see" } element)
            return false;

        if (element.Attribute(XName.Get("href")) is not { Value: [_, ..] href })
            return false;

        var inlineCode = new SeeHrefElement(href);
        elements.Add(inlineCode);
        return true;
    }

    private static Boolean TryParseSeeLangword(
        XNode candidate,
        List<XmlDocsChildElement> elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (candidate is not XElement { Name.LocalName: "see" } element)
            return false;

        if (element.Attribute(XName.Get("langword")) is not { Value: [_, ..] langword })
            return false;

        var inlineCode = new SeeLangwordElement(langword);
        elements.Add(inlineCode);
        return true;
    }

    private static String RemoveWhitespace(String text, XmlDocsProviderOptions options)
        => options.TrimmableWhitespace is [_, ..] tokens
            ? text.Replace(tokens, String.Empty)
            : text;

    private static void AddInterfaceImplementationOrOverriddenMembers(
        INamedTypeSymbol type,
        Dictionary<ISymbol, Dictionary<ISymbol, HashSet<String>>> interfaceImplementationsOrOverriddenMembers,
        CancellationToken ct)
    {
        if (interfaceImplementationsOrOverriddenMembers.ContainsKey(type))
            return;

        var memberInheritedDocsIdMap = new Dictionary<ISymbol, HashSet<String>>(SymbolEqualityComparer.Default);

        AddOverriddenMembers(type, memberInheritedDocsIdMap, ct);
        AddInterfaceImplementationMembers(type, memberInheritedDocsIdMap, ct);

        interfaceImplementationsOrOverriddenMembers.Add(type, memberInheritedDocsIdMap);
    }

    private static void AddInterfaceImplementationMembers(
        INamedTypeSymbol type,
        Dictionary<ISymbol, HashSet<String>> memberInheritedDocsIdMap,
        CancellationToken ct)
    {
        foreach (var @interface in type.AllInterfaces)
        {
            ct.ThrowIfCancellationRequested();

            var interfaceMembers = @interface.GetMembers();
            foreach (var interfaceMember in interfaceMembers)
            {
                ct.ThrowIfCancellationRequested();

                if (interfaceMember.GetDocumentationCommentId() is not { } referenceId)
                    continue;

                var implementation = type.FindImplementationForInterfaceMember(interfaceMember);
                if (!type.Equals(implementation?.ContainingType, SymbolEqualityComparer.Default))
                    continue;

                if (!memberInheritedDocsIdMap.TryGetValue(implementation, out var referenceIds))
                    memberInheritedDocsIdMap.Add(implementation, referenceIds = []);

                referenceIds.Add(referenceId);
            }
        }
    }

    private static void AddOverriddenMembers(
        INamedTypeSymbol type,
        Dictionary<ISymbol, HashSet<String>> memberInheritedDocsIdMap,
        CancellationToken ct)
    {
        foreach (var member in type.GetMembers())
        {
            ct.ThrowIfCancellationRequested();

            if (member switch
                {
                    IMethodSymbol { OverriddenMethod: { } o } => (ISymbol)o,
                    IPropertySymbol { OverriddenProperty: { } p } => p,
                    IEventSymbol { OverriddenEvent: { } e } => e,
                    _ => null
                } is not { } overridden || overridden.GetDocumentationCommentId() is not { } referenceId)
                continue;

            if (!memberInheritedDocsIdMap.TryGetValue(member, out var referenceIds))
                memberInheritedDocsIdMap.Add(member, referenceIds = []);

            referenceIds.Add(referenceId);
        }
    }

    private static Boolean TryParseInheritdoc(
        ISymbol containingSymbol,
        XElement element,
        ImmutableHashSet<String>.Builder references,
        Dictionary<ISymbol, Dictionary<ISymbol, HashSet<String>>> interfaceImplementationsOrOverriddenMembers,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (element is not { Name.LocalName: "inheritdoc" })
            return false;

        if (element.Attribute(XName.Get("cref")) is { Value: { } cref })
        {
            references.Add(cref);
            return true;
        }

        if (containingSymbol is INamedTypeSymbol containingType)
            AppendBaseTypeOrInterfaceReference(containingType, references, ct);
        else
            AppendMemberReference(containingSymbol, references, interfaceImplementationsOrOverriddenMembers, ct);

        return true;
    }

    private static void AppendMemberReference(
        ISymbol symbol,
        ICollection<String> references,
        Dictionary<ISymbol, Dictionary<ISymbol, HashSet<String>>> interfaceImplementationsOrOverriddenMembers,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (symbol.ContainingType is not { } containingType ||
            !interfaceImplementationsOrOverriddenMembers.TryGetValue(containingType, out var membersMap) ||
            !membersMap.TryGetValue(symbol, out var referenceIds))
            return;

        foreach (var referenceId in referenceIds)
            references.Add(referenceId);
    }

    private static void AppendBaseTypeOrInterfaceReference(
        INamedTypeSymbol containingType,
        ICollection<String> references,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (containingType switch
            {
                {
                    BaseType: { } baseType,
                    Interfaces: []
                } => baseType.GetDocumentationCommentId(),
                {
                    BaseType.SpecialType: SpecialType.System_Object,
                    Interfaces: [{ } singleInterface]
                } => singleInterface.GetDocumentationCommentId(),
                _ => null
            } is { } reference)
        {
            references.Add(reference);
        }
    }

    public MemberElement GetXmlDocs(INamedTypeSymbol type) => GetMemberXmlDocs(type);
    public MemberElement GetXmlDocs(IMethodSymbol method) => GetMemberXmlDocs(method);
    public MemberElement GetXmlDocs(IEventSymbol @event) => GetMemberXmlDocs(@event);
    public MemberElement GetXmlDocs(IFieldSymbol field) => GetMemberXmlDocs(field);
    public MemberElement GetXmlDocs(INamespaceSymbol @namespace) => GetMemberXmlDocs(@namespace);
    public MemberElement GetXmlDocs(IPropertySymbol property) => GetMemberXmlDocs(property);

    public MemberElement GetMemberXmlDocs(ISymbol symbol)
    {
        if (symbol.GetDocumentationCommentId() is not { } id)
            return MemberElement.Empty;

        var result = _members.GetOrAdd(
            id,
            CreateMember,
            _elements);

        return result;
    }

    private static MemberElement CreateMember(String id, Digraph<XmlDocsElement> elements)
    {
        SummaryElement summary = SummaryElement.Empty;
        ReturnsElement returns = ReturnsElement.Empty;
        RemarksElement remarks = RemarksElement.Empty;
        ExampleElement example = ExampleElement.Empty;
        List<ParamElement> @params = [];
        List<TypeparamElement> typeparams = [];
        List<ExceptionElement> exceptions = [];

        foreach (var element in elements.GetChildren(id))
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
            @params is [] ? [] : new XmlDocsElements<ParamElement>(@params),
            typeparams is [] ? [] : new XmlDocsElements<TypeparamElement>(typeparams),
            exceptions is [] ? [] : new XmlDocsElements<ExceptionElement>(exceptions)
        );

        return result;
    }

    private static Boolean TryGetMemberElements(
        ISymbol symbol,
        [NotNullWhen(true)] out IEnumerable<XElement>? elements,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        elements = null;

        if (symbol.GetDocumentationCommentXml(
                preferredCulture: options.PreferredCulture,
                expandIncludes: true,
                cancellationToken: ct) is not [_, ..] source)
            return false;

        try
        {
            if (XDocument.Parse(source).Root is not { HasElements: true } root)
                return false;

            elements = root.Elements();
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}
