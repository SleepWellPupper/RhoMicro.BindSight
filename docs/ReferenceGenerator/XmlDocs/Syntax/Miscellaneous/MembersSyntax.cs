namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record MembersSyntax(
    SyntaxChildren<MemberSyntax> Children)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Members)
{
    [field: MaybeNull] public static MembersSyntax Empty => field ??= new MembersSyntax([]);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
