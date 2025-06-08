namespace ReferenceGenerator.Astro;

using System.Text;
using ReferenceGenerator.XmlDocs;

internal readonly struct AstroXmlDocsVisitor(
    StringBuilder builder,
    AstroReferencePathContext referencePaths)
    : IXmlDocsNodeVisitor
{
    public void Visit(MemberNode memberNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        foreach (var child in memberNode.GetChildren(ct))
        {
            ct.ThrowIfCancellationRequested();

            child.Accept(this, ct);
        }
    }

    public void Visit(SummaryNode summaryNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        foreach (var child in summaryNode.GetChildren(ct))
        {
            ct.ThrowIfCancellationRequested();

            child.Accept(this, ct);
        }
    }

    public void Visit(TextNode textNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        builder.Append(textNode.OuterText.Trim().Replace("\n", String.Empty));
    }

    public void Visit(UnknownNode unknownNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        builder.Append($" `{unknownNode.OuterText}` ");
    }

    public void Visit(SeeNode seeNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!seeNode.TryGetCref(out var referencedSymbol))
            return;

        var anchorHref = referencePaths.GetPaths(referencedSymbol).AnchorHref;
        var text = referencedSymbol.ToDisplayString(SymbolDisplayFormats.SimpleGenericName);
        builder.Append($" [{text}]({anchorHref}) ");
    }

    public void Visit(TypeParamRefNode typeParamRefNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!typeParamRefNode.TryGetTypeParameter(out var typeParameter))
            return;

        var anchorHref = referencePaths.GetPaths(typeParameter).AnchorHref;
        var text = typeParameter.Name;
        builder.Append($" [{text}]({anchorHref}) ");
    }

    public void Visit(InheritDocNode inheritDocNode, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        inheritDocNode.ReferencedDocs.Accept(this, ct);
    }
}
