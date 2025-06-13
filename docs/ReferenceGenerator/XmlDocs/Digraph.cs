using System.Collections.Immutable;
using System.Text;

namespace ReferenceGenerator.XmlDocs;

internal sealed class Digraph<TChild>
{
    internal Digraph(Dictionary<String, ImmutableHashSet<TChild>> nodes) => _nodes = nodes;

    private readonly Dictionary<String, ImmutableHashSet<TChild>> _nodes;

    public ImmutableHashSet<TChild> GetChildren(String id) => _nodes.GetValueOrDefault(id, []);

    public override String ToString()
    {
        var builder = new StringBuilder();

        foreach (var (id, children) in _nodes)
        {
            builder.AppendLine(id);
            var i = 0;
            foreach (var child in children)
            {
                var prefix = i++ == children.Count - 1
                    ? "\u2514\u2500"
                    : "\u251c\u2500";
                builder.AppendLine($"{prefix}{child}");
            }
        }

        var result = builder.ToString();
        return result;
    }
}
