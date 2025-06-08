namespace ReferenceGenerator.XmlDocs;

using System.Collections.Immutable;

public sealed class DocSymbol(
    DocSyntax syntax,
    AssemblySymbol assembly,
    IReadOnlyList<MemberSymbol> members)
    : XmlDocsSymbol<DocSyntax>(syntax, XmlDocsSymbolKind.Doc)
{
    public AssemblySymbol Assembly => assembly;
    public IReadOnlyList<MemberSymbol> Members => members;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
