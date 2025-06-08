namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record RemarksSyntax(
    NestedElementsOrInheritdocSyntax Child)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Remarks)
{
    [field: MaybeNull]
    public static RemarksSyntax Empty
        => field ??= new RemarksSyntax(NestedElementsOrInheritdocSyntax.Empty);

    [field: MaybeNull]
    public static RemarksSyntax Inherited
        => field ??= new RemarksSyntax(NestedElementsOrInheritdocSyntax.Inherited);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
