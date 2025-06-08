namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record BlockCodeSyntax(
    NestedElementsSyntax NestedElements)
    : XmlDocsSyntax(XmlDocsSyntaxKind.BlockCode)
{
    [field: MaybeNull]
    public static BlockCodeSyntax Empty
        => field ??= new BlockCodeSyntax(NestedElementsSyntax.Empty);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
