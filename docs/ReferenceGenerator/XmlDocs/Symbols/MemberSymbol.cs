namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class MemberSymbol(
    MemberSyntax syntax,
    SummarySymbol summary,
    ReturnsSymbol returns,
    RemarksSymbol remarks,
    IReadOnlyList<ParamSymbol> parameters,
    IReadOnlyList<TypeparamSymbol> typeParameters,
    InheritdocSyntax? inheritdoc)
    : XmlDocsSymbol<MemberSyntax>(syntax, XmlDocsSymbolKind.Member)
{
    public SummarySymbol Summary { get; } = summary;
    public ReturnsSymbol Returns { get; } = returns;
    public RemarksSymbol Remarks { get; } = remarks;
    public IReadOnlyList<ParamSymbol> Parameters { get; } = parameters;
    public IReadOnlyList<TypeparamSymbol> TypeParameters { get; } = typeParameters;
    public InheritdocSyntax? Inheritdoc { get; } = inheritdoc;

    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
