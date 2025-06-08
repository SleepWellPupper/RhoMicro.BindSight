namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record InheritdocSyntax(String Cref)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Inheritdoc)
{
    [field: MaybeNull]
    public static InheritdocSyntax Empty
        => field ??= new InheritdocSyntax(Cref: String.Empty);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
