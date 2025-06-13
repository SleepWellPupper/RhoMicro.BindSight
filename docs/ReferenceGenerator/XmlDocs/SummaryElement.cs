namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record SummaryElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsElement(XmlDocsKind.Summary)
{
    public static SummaryElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
