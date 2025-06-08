namespace ReferenceGenerator.XmlDocs;

public sealed record ParamSyntax(
    String Name,
    NestedElementsOrInheritdocSyntax Child)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Param)
{
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
