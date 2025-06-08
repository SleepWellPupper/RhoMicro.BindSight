namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

public sealed class XmlDocsSemanticModel
{
    private XmlDocsSemanticModel(XmlDocsSemanticModelOptions options)
    {
        Options = options;
        _symbolFactory = new SymbolFactory(this);
    }

    private readonly SymbolFactory _symbolFactory;
    private readonly Dictionary<String, (IXmlDocsSymbol, ISymbol)> _symbols = [];
    public XmlDocsSemanticModelOptions Options { get; }

    public static XmlDocsSemanticModel Create(
        Compilation compilation,
        XmlDocsSemanticModelOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = new XmlDocsSemanticModel(options);
        result.Add(compilation.Assembly, [], ct);

        return result;
    }

    private void Add(
        IAssemblySymbol assembly,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(assembly))
            return;

        Add(assembly, ct);
        Add(assembly.GlobalNamespace, mappedSymbols, ct);
    }

    private void Add(
        INamespaceSymbol @namespace,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(@namespace))
            return;

        Add(@namespace, ct);

        foreach (var type in @namespace.GetTypeMembers())
        {
            ct.ThrowIfCancellationRequested();

            Add(type, mappedSymbols, ct);
        }

        foreach (var namespaceMember in @namespace.GetNamespaceMembers())
        {
            ct.ThrowIfCancellationRequested();

            Add(namespaceMember, mappedSymbols, ct);
        }
    }

    private void Add(
        ITypeSymbol type,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(type))
            return;

        Add(type, ct);

        foreach (var member in type.GetMembers())
        {
            ct.ThrowIfCancellationRequested();

            Add(member, mappedSymbols, ct);
        }

        if (type is INamedTypeSymbol { TypeParameters: var typeParameters })
        {
            foreach (var typeParameter in typeParameters)
            {
                ct.ThrowIfCancellationRequested();

                Add(typeParameter, mappedSymbols, ct);
            }
        }
    }

    public String CreateParameterId(String prefix, String suffix)
        => $"{prefix}${suffix}";

    private void Add(
        ITypeParameterSymbol typeParameter,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(typeParameter))
            return;

        if (typeParameter.ContainingSymbol.GetDocumentationCommentId() is not { } prefix)
            return;

        var id = $"{prefix}${typeParameter.Name}";

        Add(typeParameter, id, ct);
    }

    private void Add(
        IParameterSymbol parameter,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(parameter))
            return;

        if (parameter.ContainingSymbol.GetDocumentationCommentId() is not { } prefix)
            return;

        var id = $"{prefix}${parameter.Name}";

        Add(parameter, id, ct);
    }

    private void Add(
        ISymbol symbol,
        HashSet<ISymbol> mappedSymbols,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!mappedSymbols.Add(symbol))
            return;

        Add(symbol, ct);

        if (symbol is IMethodSymbol
            {
                Parameters: var parameters,
                TypeParameters: var typeParameters
            })
        {
            foreach (var parameter in parameters)
            {
                ct.ThrowIfCancellationRequested();

                Add(parameter, mappedSymbols, ct);
            }

            foreach (var typeParameter in typeParameters)
            {
                ct.ThrowIfCancellationRequested();

                Add(typeParameter, mappedSymbols, ct);
            }
        }
    }

    private void Add(ISymbol symbol, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (symbol.GetDocumentationCommentId() is not { } id)
            return;

        Add(symbol, id, ct);
    }

    private void Add(
        ISymbol symbol,
        String id,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (_symbols.ContainsKey(id))
            return;

        if (symbol.GetDocumentationCommentXml(
                Options.PreferredCulture,
                expandIncludes: true,
                ct) is not { } source)
            return;

        if (XmlDocsParser.Create(source, Options.XmlDocsParserOptions).Parse(ct) is not ParseResult.Success success)
            return;

        switch (success)
        {
            case ParseResult.Success.MemberResult { Syntax: var member }:
                var memberSymbol = _symbolFactory.Member(member, ct);
                _symbols[id] = (memberSymbol, symbol);
                break;
            case ParseResult.Success.DocResult { Syntax: var doc }:
                var docSymbol = _symbolFactory.Doc(doc, ct);
                _symbols[id] = (docSymbol, symbol);
                break;
        }
    }


    private Boolean TryGetSymbols(
        String id,
        [NotNullWhen(true)] out IXmlDocsSymbol? symbol,
        [NotNullWhen(true)] out ISymbol? roslynSymbol)
    {
        if (_symbols.TryGetValue(id, out var t))
        {
            (symbol, roslynSymbol) = t;
            return true;
        }

        symbol = null;
        roslynSymbol = null;
        return false;
    }

    public Boolean TryGetDocumentationSymbol(String id, [NotNullWhen(true)] out IXmlDocsSymbol? symbol)
        => TryGetSymbols(id, out symbol, out _);

    public IXmlDocsSymbol? GetDocumentationSymbol(String id)
        => TryGetDocumentationSymbol(id, out var s)
            ? s
            : null;

    public Boolean TryGetRoslynSymbol(String id, [NotNullWhen(true)] out ISymbol? symbol)
        => TryGetSymbols(id, out _, out symbol);

    public ISymbol? GetSymbol(String id)
        => TryGetRoslynSymbol(id, out var s)
            ? s
            : null;

    // TODO symbol factory
}

public sealed class SymbolFactory(XmlDocsSemanticModel semanticModel)
{
    public DocSymbol Doc(IAssemblySymbol assembly, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var source = assembly.GetDocumentationCommentXml(
            preferredCulture: semanticModel.Options.PreferredCulture,
            expandIncludes: true,
            cancellationToken: ct) ?? String.Empty;
        var parseResult = XmlDocsParser.Create(source, semanticModel.Options.XmlDocsParserOptions).Parse(ct);
        var syntax = parseResult is ParseResult.Success.DocResult { Syntax: var r }
            ? r
            : SyntaxFactory.Doc(assembly.Name);
        var result = Doc(syntax, ct);

        return result;
    }

    public DocSymbol Doc(DocSyntax syntax, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var assembly = Assembly(syntax.Assembly, ct);
        var members = ImmutableArray.CreateBuilder<MemberSymbol>(syntax.Members.Children.Count);

        foreach (var child in syntax.Members.Children)
        {
            ct.ThrowIfCancellationRequested();

            var member = Member(child, ct);
            members.Add(member);
        }

        var result = new DocSymbol(syntax, assembly, members.MoveToImmutable());
        return result;
    }

    public AssemblySymbol Assembly(AssemblySyntax syntax, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = new AssemblySymbol(syntax);

        return result;
    }

    public MemberSymbol Member(MemberSyntax syntax, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var members = ImmutableArray.CreateBuilder<IXmlDocsSymbol>();

        foreach (var element in syntax.Elements)
        {
            ct.ThrowIfCancellationRequested();

            var elementChild = element.Child;

            switch (elementChild.Kind)
            {
                case XmlDocsSyntaxKind.Summary:
                    members.Add(Summary((SummarySyntax)elementChild, ct));
                    break;
                case XmlDocsSyntaxKind.Param:
                    members.Add(Param((ParamSyntax)elementChild, ct));
                    break;
                case XmlDocsSyntaxKind.Returns:
                    members.Add(Returns((ReturnsSyntax)elementChild, ct));
                    break;
                case XmlDocsSyntaxKind.Remarks:
                    members.Add(Remarks((RemarksSyntax)elementChild, ct));
                    break;
                case XmlDocsSyntaxKind.Typeparam:
                    members.Add(Typeparam((TypeparamSyntax)elementChild, ct));
                    break;
            }
        }

        if (syntax.Inheritdoc is { } inheritdoc)
            members.Add(Inheritdoc(inheritdoc, ct));

        var result = new MemberSymbol(syntax, members.MoveToImmutable());
        return result;
    }

    private ImmutableArray<IXmlDocsSymbol> GetNestedElementsOrInheritDoc(
        NestedElementsOrInheritdocSyntax syntax,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (syntax.Child.Kind is XmlDocsSyntaxKind.Inheritdoc)
            return [Inheritdoc((InheritdocSyntax)syntax.Child, ct)];

        var children = ((NestedElementsSyntax)syntax.Child).Children;
        var result = ImmutableArray.CreateBuilder<IXmlDocsSymbol>(children.Count);
        foreach (var child in children)
        {
            switch (child.Kind)
            {
                case XmlDocsSyntaxKind.Text:
                    result.Add(Text((TextSyntax)child));
                    break;
                case XmlDocsSyntaxKind.InlineCode:
                    result.Add(InlineCode((InlineCodeSyntax)child));
                    break;
                case XmlDocsSyntaxKind.BlockCode:
                    result.Add(BlockCode((BlockCodeSyntax)child));
                    break;
                case XmlDocsSyntaxKind.See:
                    result.Add(See((SeeSyntax)child));
                    break;
                case XmlDocsSyntaxKind.Paramref:
                    result.Add(Paramref((ParamrefSyntax)child));
                    break;
                case XmlDocsSyntaxKind.Typeparamref:
                    result.Add(Typeparamref((TypeparamrefSyntax)child));
                    break;
            }
        }

        return result.MoveToImmutable();
    }

    public SummarySymbol Summary(SummarySyntax syntax, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var children = GetNestedElementsOrInheritDoc(syntax.Child, ct);
        var result = new SummarySymbol(syntax, children);

        return result;
    }

    public ParamSymbol Param(ParamSyntax syntax, String parentId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var id = semanticModel.CreateParameterId(parentId, syntax.Name);
        var reference = new XmlDocsSymbolReference(semanticModel, id);
        var result = new ParamSymbol(syntax, reference);

        return result;
    }

    public ReturnsSymbol Returns(ReturnsSyntax syntax, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var children = GetNestedElementsOrInheritDoc(syntax.Child, ct);
        var result = new ReturnsSymbol(syntax, children);

        return result;
    }

    public RemarksSymbol Remarks(RemarksSyntax syntax, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var children = GetNestedElementsOrInheritDoc(syntax.Child, ct);
        var result = new RemarksSymbol(syntax, children);

        return result;
    }

    public TypeparamSymbol Typeparam(TypeparamSyntax syntax, String parentId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var id = semanticModel.CreateParameterId(parentId, syntax.Name);
        var reference = new XmlDocsSymbolReference(semanticModel, id);
        var result = new TypeparamSymbol(syntax, reference);
        return result;
    }
}
