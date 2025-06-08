namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record InlineCodeSyntax(NestedElementsSyntax NestedElements) : XmlDocsSyntax(XmlDocsSyntaxKind.InlineCode)
{
    [field: MaybeNull]
    public static InlineCodeSyntax Empty
        => field ??= new InlineCodeSyntax(NestedElementsSyntax.Empty);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
