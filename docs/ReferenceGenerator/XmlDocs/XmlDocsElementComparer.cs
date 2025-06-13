namespace ReferenceGenerator.XmlDocs;

internal sealed class XmlDocsElementComparer<TElement> : IComparer<TElement>
    where TElement : XmlDocsElement
{
    private XmlDocsElementComparer()
    {
    }

    public static XmlDocsElementComparer<TElement> Instance { get; } = new();

    /// <inheritdoc />
    public Int32 Compare(TElement? x, TElement? y)
    {
        if (ReferenceEquals(x, y))
            return 0;

        if (y is null)
            return 1;

        if (x is null)
            return -1;

        return (x, y) switch
        {
            (ParamElement { Name: var xName }, ParamElement { Name: var yName })
                => StringComparer.Ordinal.Compare(xName, yName),
            (TypeparamElement { Name: var xName }, TypeparamElement { Name: var yName })
                => StringComparer.Ordinal.Compare(xName, yName),
            (XmlDocsChildElement, XmlDocsChildElement) => 0,
            _ => x.Kind.CompareTo(y.Kind)
        };
    }
}
