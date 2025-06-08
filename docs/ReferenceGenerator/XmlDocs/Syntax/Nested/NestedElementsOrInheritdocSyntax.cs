namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record NestedElementsOrInheritdocSyntax : XmlDocsSyntax
{
    public NestedElementsOrInheritdocSyntax(NestedElementsSyntax child)
        : base(XmlDocsSyntaxKind.NestedElementsOrInheritdoc)
        => Child = child;

    public NestedElementsOrInheritdocSyntax(InheritdocSyntax child)
        : base(XmlDocsSyntaxKind.NestedElementsOrInheritdoc)
        => Child = child;

    [field: MaybeNull]
    public static NestedElementsOrInheritdocSyntax Empty =>
        field ??= new NestedElementsOrInheritdocSyntax(NestedElementsSyntax.Empty);

    [field: MaybeNull]
    public static NestedElementsOrInheritdocSyntax Inherited =>
        field ??= new NestedElementsOrInheritdocSyntax(InheritdocSyntax.Empty);

    public XmlDocsSyntax Child { get; }

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
