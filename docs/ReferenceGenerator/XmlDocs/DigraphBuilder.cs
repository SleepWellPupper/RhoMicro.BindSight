using System.Collections.Immutable;
using System.Text;

namespace ReferenceGenerator.XmlDocs;

internal class DigraphBuilder<TChild>(IEqualityComparer<TChild> childEqualityComparer)
{
    public DigraphBuilder() : this(EqualityComparer<TChild>.Default)
    {
    }

    private readonly Dictionary<String, DigraphNode<TChild>> _nodes = [];

    public ImmutableHashSet<String> GetReferenceIds(String id)
        => GetNode(id) is { ReferenceIds: { } referenceIds }
            ? referenceIds
            : [];

    public DigraphBuilder<TChild> Add(
        String id,
        IEnumerable<TChild> children,
        IEnumerable<String> references)
    {
        var node = new DigraphNode<TChild>(
            id,
            children.ToImmutableHashSet(childEqualityComparer),
            [..references.Distinct()]);

        _nodes[id] = node;

        return this;
    }

    private DigraphNode<TChild>? GetNode(String id) => _nodes.GetValueOrDefault(id);

    public Digraph<TChild> Build(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var resolvedNodes = new Dictionary<String, ImmutableHashSet<TChild>>();
        var children = ImmutableHashSet.CreateBuilder<TChild>(childEqualityComparer);

        foreach (var unresolvedNode in _nodes.Values)
        {
            ct.ThrowIfCancellationRequested();

            if (unresolvedNode.ReferenceIds.IsEmpty)
            {
                resolvedNodes.Add(unresolvedNode.Id, unresolvedNode.Children);
                continue;
            }

            children.Clear();
            GetTransitivelyReferencedChildren(unresolvedNode, children, ct);

            resolvedNodes.Add(unresolvedNode.Id, children.ToImmutable());
        }

        var result = new Digraph<TChild>(resolvedNodes);
        return result;
    }

    private void GetTransitivelyReferencedChildren(
        DigraphNode<TChild> node,
        ImmutableHashSet<TChild>.Builder children,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var queue = new Queue<DigraphNode<TChild>>();
        queue.Enqueue(node);
        var included = new HashSet<String>() { node.Id };
        while (queue.TryDequeue(out var next))
        {
            foreach (var child in next.Children)
                children.Add(child);

            foreach (var referenceId in next.ReferenceIds)
            {
                if (GetNode(referenceId) is not { } reference)
                    continue;

                if (included.Add(referenceId))
                    queue.Enqueue(reference);
            }
        }
    }

    public override String ToString()
    {
        var builder = new StringBuilder();

        foreach (var node in _nodes.Values)
        {
            builder.AppendLine(node.Id);
            var i = 0;
            foreach (var child in node.Children)
            {
                var prefix = node.ReferenceIds.IsEmpty && i++ == node.Children.Count - 1
                    ? "\u2514\u2500"
                    : "\u251c\u2500";
                builder.AppendLine($"{prefix}{child}");
            }

            i = 0;
            foreach (var referenceId in node.ReferenceIds)
            {
                var prefix = i == node.ReferenceIds.Count - 1
                    ? "\u2514>"
                    : "\u251c>";
                builder.AppendLine($"{prefix}{referenceId}");
            }
        }

        var result = builder.ToString();
        return result;
    }
}
