namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record ExampleElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsElement(XmlDocsKind.Example)
{
    public static ExampleElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
