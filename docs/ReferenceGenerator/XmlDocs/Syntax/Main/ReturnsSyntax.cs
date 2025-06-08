namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record ReturnsSyntax(
    NestedElementsOrInheritdocSyntax Child)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Returns)
{
    [field: MaybeNull]
    public static ReturnsSyntax Empty
        => field ??= new ReturnsSyntax(NestedElementsOrInheritdocSyntax.Empty);

    [field: MaybeNull]
    public static ReturnsSyntax Inherited
        => field ??= new ReturnsSyntax(NestedElementsOrInheritdocSyntax.Inherited);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
