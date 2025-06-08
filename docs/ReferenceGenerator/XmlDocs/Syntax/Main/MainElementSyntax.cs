namespace ReferenceGenerator.XmlDocs;

using System.Diagnostics.CodeAnalysis;

public sealed record MainElementSyntax : XmlDocsSyntax
{
    public MainElementSyntax(SummarySyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    public MainElementSyntax(ParamSyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    public MainElementSyntax(ReturnsSyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    public MainElementSyntax(RemarksSyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    public MainElementSyntax(TypeparamSyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    public MainElementSyntax(InheritdocSyntax child)
        : base(XmlDocsSyntaxKind.MainElement)
        => Child = child;

    [field: MaybeNull]
    public static MainElementSyntax EmptySummary => field ??=
        new MainElementSyntax(SummarySyntax.Empty);

    [field: MaybeNull]
    public static MainElementSyntax InheritedSummary => field ??=
        new MainElementSyntax(SummarySyntax.Inherited);

    [field: MaybeNull]
    public static MainElementSyntax EmptyReturns => field ??=
        new MainElementSyntax(ReturnsSyntax.Empty);

    [field: MaybeNull]
    public static MainElementSyntax InheritedReturns => field ??=
        new MainElementSyntax(ReturnsSyntax.Inherited);

    [field: MaybeNull]
    public static MainElementSyntax EmptyRemarks => field ??=
        new MainElementSyntax(RemarksSyntax.Empty);

    [field: MaybeNull]
    public static MainElementSyntax InheritedRemarks => field ??=
        new MainElementSyntax(RemarksSyntax.Inherited);

    [field: MaybeNull]
    public static MainElementSyntax EmptyInheritdoc => field ??=
        new MainElementSyntax(InheritdocSyntax.Empty);

    public XmlDocsSyntax Child { get; }

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);

    public static implicit operator MainElementSyntax(SummarySyntax child) => new MainElementSyntax(child);
    public static implicit operator MainElementSyntax(ParamSyntax child) => new MainElementSyntax(child);
    public static implicit operator MainElementSyntax(ReturnsSyntax child) => new MainElementSyntax(child);
    public static implicit operator MainElementSyntax(RemarksSyntax child) => new MainElementSyntax(child);
    public static implicit operator MainElementSyntax(TypeparamSyntax child) => new MainElementSyntax(child);
    public static implicit operator MainElementSyntax(InheritdocSyntax child) => new MainElementSyntax(child);
}
