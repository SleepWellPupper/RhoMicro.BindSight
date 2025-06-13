namespace ReferenceGenerator.XmlDocs;

public sealed record BlockCodeElement(
    XmlDocsElements<XmlDocsChildElement> Elements)
    : XmlDocsChildElement(XmlDocsKind.BlockCode)
{
    public static BlockCodeElement Empty { get; } = new([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
