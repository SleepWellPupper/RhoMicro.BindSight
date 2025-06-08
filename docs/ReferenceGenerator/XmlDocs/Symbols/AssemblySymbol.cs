namespace ReferenceGenerator.XmlDocs;

public sealed class AssemblySymbol(
    AssemblySyntax syntax)
    : XmlDocsSymbol<AssemblySyntax>(syntax, XmlDocsSymbolKind.Assembly)
{
    public String Name => Syntax.Name.Text.Text;
    public override void Accept<TVisitor>(TVisitor visitor) => visitor.Visit(this);
}
