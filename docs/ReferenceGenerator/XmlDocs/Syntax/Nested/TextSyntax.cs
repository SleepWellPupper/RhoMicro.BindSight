namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record TextSyntax(String Text) : XmlDocsSyntax(XmlDocsSyntaxKind.Text)
{
    [field: MaybeNull]
    public static TextSyntax Empty
        => field ??= new TextSyntax(Text: String.Empty);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
