namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record NestedElementSyntax : XmlDocsSyntax
{
    public NestedElementSyntax(TextSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    public NestedElementSyntax(InlineCodeSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    public NestedElementSyntax(BlockCodeSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    public NestedElementSyntax(SeeSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    public NestedElementSyntax(ParamrefSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    public NestedElementSyntax(TypeparamrefSyntax child)
        : base(XmlDocsSyntaxKind.NestedElement)
        => Child = child;

    [field: MaybeNull]
    public static NestedElementSyntax EmptyBlockCode => field ??=
        new NestedElementSyntax(BlockCodeSyntax.Empty);

    [field: MaybeNull]
    public static NestedElementSyntax EmptyText => field ??=
        new NestedElementSyntax(TextSyntax.Empty);

    [field: MaybeNull]
    public static NestedElementSyntax EmptyInlineCode => field ??=
        new NestedElementSyntax(InlineCodeSyntax.Empty);

    public XmlDocsSyntax Child { get; }

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
