namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

public sealed class XmlDocsSemanticModel
{
    private XmlDocsSemanticModel()
    {
    }

    private readonly Dictionary<String, ISymbol> _idMap = [];
    public IEnumerable<ISymbol> KnownSymbols => _idMap.Values;

    public static XmlDocsSemanticModel Create(Compilation compilation, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = new XmlDocsSemanticModel();
        result.AddNamespace(compilation.GlobalNamespace, ct);

        return result;
    }

    public Boolean TryGetSymbol(
        String documentationCommentId,
        [NotNullWhen(true)] out ISymbol? symbol)
        => TryGetSymbol<ISymbol>(documentationCommentId, out symbol);

    public Boolean TryGetSymbol<TSymbol>(
        String documentationCommentId,
        [NotNullWhen(true)] out TSymbol? symbol)
        where TSymbol : class, ISymbol
    {
        if (_idMap.TryGetValue(documentationCommentId, out var s) && s is TSymbol t)
        {
            symbol = t;
            return true;
        }

        symbol = null;
        return false;
    }

    private void AddNamespace(INamespaceSymbol @namespace, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (@namespace.GetDocumentationCommentId() is { } id)
            _idMap[id] = @namespace;

        foreach (var type in @namespace.GetMembers().OfType<INamedTypeSymbol>())
        {
            ct.ThrowIfCancellationRequested();
            AddType(type, ct);
        }

        foreach (var namespaceMember in @namespace.GetNamespaceMembers())
        {
            ct.ThrowIfCancellationRequested();
            AddNamespace(namespaceMember, ct);
        }
    }

    private void AddType(INamedTypeSymbol type, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (type.GetDocumentationCommentId() is { } id)
            _idMap[id] = type;

        foreach (var member in type.GetMembers())
        {
            ct.ThrowIfCancellationRequested();

            switch (member)
            {
                case IEventSymbol
                    or IFieldSymbol
                    or IMethodSymbol
                    or IPropertySymbol:
                    AddMember(member, ct);
                    break;
                case INamedTypeSymbol typeMember:
                    AddType(typeMember, ct);
                    break;
            }
        }
    }

    private void AddMember(ISymbol member, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (member.GetDocumentationCommentId() is { } id)
            _idMap[id] = member;
    }
}
