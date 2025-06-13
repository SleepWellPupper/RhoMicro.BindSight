namespace ReferenceGenerator;

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using XmlDocs;

internal sealed class MemberDocumentationModel(
    ISymbol symbol,
    XmlDocsContext docsContext)
{
    public ISymbol Symbol => symbol;
    public XmlDocsContext DocsContext => docsContext;
    public MemberElement Docs => docsContext.Provider.GetMemberXmlDocs(Symbol);

    [field: MaybeNull] public String Signature => field ??= symbol.ToDisplayString(SymbolDisplayFormats.Signature);
}
