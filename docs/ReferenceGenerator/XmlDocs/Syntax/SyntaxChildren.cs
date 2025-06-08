namespace ReferenceGenerator.XmlDocs;

using System.Collections;
using System.Runtime.CompilerServices;

public static class SyntaxChildren
{
    public static SyntaxChildren<TSyntax> Create<TSyntax>(ReadOnlySpan<TSyntax> values)
        where TSyntax : XmlDocsSyntax
        => values is []
            ? SyntaxChildren<TSyntax>.Empty
            : new SyntaxChildren<TSyntax>((List<TSyntax>)[..values]);
}

[CollectionBuilder(typeof(SyntaxChildren), nameof(SyntaxChildren.Create))]
public sealed class SyntaxChildren<TSyntax>(IReadOnlyList<TSyntax> elements)
    : IReadOnlyList<TSyntax>, IEquatable<SyntaxChildren<TSyntax>>
    where TSyntax : XmlDocsSyntax
{
    public static SyntaxChildren<TSyntax> Empty { get; } = new([]);

    public Boolean Equals(SyntaxChildren<TSyntax>? other)
    {
        if (other is not { Count: var c } || c != Count)
            return false;

        var result = other.SequenceEqual(this);

        return result;
    }

    public override Boolean Equals(Object? obj)
        => Equals(obj as SyntaxChildren<TSyntax>);

    public override Int32 GetHashCode()
    {
        var hc = new HashCode();

        foreach (var child in this)
            hc.Add(child);

        var result = hc.ToHashCode();

        return result;
    }

    public IEnumerator<TSyntax> GetEnumerator() => elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)elements).GetEnumerator();

    public Int32 Count => elements.Count;

    public TSyntax this[Int32 index] => elements[index];
}
