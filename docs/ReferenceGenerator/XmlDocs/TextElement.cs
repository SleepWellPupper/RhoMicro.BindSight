namespace ReferenceGenerator.XmlDocs;

public sealed record TextElement(
    String Value)
    : XmlDocsChildElement(XmlDocsKind.Text)
{
    public static TextElement Empty { get; } = new(String.Empty);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
    public override String ToString() => base.ToString();
}
