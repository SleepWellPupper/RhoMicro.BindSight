namespace ReferenceGenerator.XmlDocs;

using System.Collections;
using System.Runtime.CompilerServices;

public static class XmlDocsElements
{
    public static XmlDocsElements<TSyntax> Create<TSyntax>(ReadOnlySpan<TSyntax> values)
        where TSyntax : XmlDocsElement
        => values is []
            ? XmlDocsElements<TSyntax>.Empty
            : new XmlDocsElements<TSyntax>((List<TSyntax>) [..values]);
}

[CollectionBuilder(typeof(XmlDocsElements), nameof(XmlDocsElements.Create))]
public sealed class XmlDocsElements<TElement>(IEnumerable<TElement> elements)
    : IReadOnlyList<TElement>, IEquatable<XmlDocsElements<TElement>>
    where TElement : XmlDocsElement
{
    private readonly IReadOnlyList<TElement> _elements =
        elements.Order(XmlDocsElementComparer<TElement>.Instance).ToList();

    public static XmlDocsElements<TElement> Empty { get; } = new([]);

    public XmlDocsElements<TElement> Concat(XmlDocsElements<TElement> other)
        => new(_elements.Concat(other).ToList());

    public Boolean Equals(XmlDocsElements<TElement>? other)
    {
        if (other is not { Count: var c } || c != Count)
            return false;

        var result = other.SequenceEqual(this);

        return result;
    }

    public override Boolean Equals(Object? obj)
        => Equals(obj as XmlDocsElements<TElement>);

    public override Int32 GetHashCode()
    {
        var hc = new HashCode();

        foreach (var child in this)
            hc.Add(child);

        var result = hc.ToHashCode();

        return result;
    }

    public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_elements).GetEnumerator();

    public Int32 Count => _elements.Count;

    public TElement this[Int32 index] => _elements[index];
}
