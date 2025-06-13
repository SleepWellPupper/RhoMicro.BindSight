namespace ReferenceGenerator.XmlDocs;

internal sealed class XmlDocsElementTypeComparer : IEqualityComparer<XmlDocsElement>
{
    private XmlDocsElementTypeComparer()
    {
    }

    public static XmlDocsElementTypeComparer Instance { get; } = new();

    public Boolean Equals(XmlDocsElement? x, XmlDocsElement? y) =>
        (x, y) switch
        {
            (null, null) => true,
            (SummaryElement, SummaryElement) => true,
            (RemarksElement, RemarksElement) => true,
            (ReturnsElement, ReturnsElement) => true,
            (ExampleElement, ExampleElement) => true,
            (ParamElement { Name: { } xName }, ParamElement { Name: { } yName }) => xName == yName,
            (TypeparamElement { Name: { } xName }, TypeparamElement { Name: { } yName }) => xName == yName,
            (ExceptionElement { Cref: { } xCref }, ExceptionElement { Cref: { } yCref }) => xCref == yCref,
            _ => EqualityComparer<XmlDocsElement>.Default.Equals(x, y)
        };

    public Int32 GetHashCode(XmlDocsElement obj) =>
        obj switch
        {
            SummaryElement => typeof(SummaryElement).GetHashCode(),
            RemarksElement => typeof(RemarksElement).GetHashCode(),
            ReturnsElement => typeof(ReturnsElement).GetHashCode(),
            ExampleElement => typeof(ExampleElement).GetHashCode(),
            ParamElement { Name: { } name } => HashCode.Combine(typeof(ParamElement), name),
            TypeparamElement { Name: { } name } => HashCode.Combine(typeof(TypeparamElement), name),
            _ => EqualityComparer<XmlDocsElement>.Default.GetHashCode(obj)
        };
}
