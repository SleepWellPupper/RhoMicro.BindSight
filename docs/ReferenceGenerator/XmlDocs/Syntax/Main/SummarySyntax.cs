namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record SummarySyntax(NestedElementsOrInheritdocSyntax Child)
    : XmlDocsSyntax(XmlDocsSyntaxKind.Summary)
{
    [field: MaybeNull]
    public static SummarySyntax Empty
        => field ??= new SummarySyntax(NestedElementsOrInheritdocSyntax.Empty);

    [field: MaybeNull]
    public static SummarySyntax Inherited
        => field ??= new SummarySyntax(NestedElementsOrInheritdocSyntax.Inherited);

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
