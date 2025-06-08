namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record NestedElementsSyntax(
    SyntaxChildren<NestedElementSyntax> Children)
    : XmlDocsSyntax(XmlDocsSyntaxKind.NestedElements)
{
    [field: MaybeNull]
    public static NestedElementsSyntax Empty
        => field ??= new NestedElementsSyntax([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
