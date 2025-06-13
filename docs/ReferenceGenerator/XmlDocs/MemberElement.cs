namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record MemberElement(
    SummaryElement Summary,
    ReturnsElement Returns,
    ExampleElement Example,
    RemarksElement Remarks,
    XmlDocsElements<ParamElement> Params,
    XmlDocsElements<TypeparamElement> Typeparams,
    XmlDocsElements<ExceptionElement> Exceptions
) : XmlDocsElement(XmlDocsKind.Member)
{
    public static MemberElement Empty { get; } = new(
        SummaryElement.Empty,
        ReturnsElement.Empty,
        ExampleElement.Empty,
        RemarksElement.Empty,
        [],
        [],
        []);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
