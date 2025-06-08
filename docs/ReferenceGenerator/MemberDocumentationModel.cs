namespace ReferenceGenerator;

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using XmlDocs;

internal sealed class MemberDocumentationModel(
    ISymbol symbol,
    Compilation compilation,
    XmlDocsParseNodeOptions xmlDocsParseNodeOptions)
{
    public ISymbol Symbol => symbol;

    [field: MaybeNull]
    public MemberSyntax? Documentation => field ??= XmlDocsParser.Create(symbol, compilation, xmlDocsParseNodeOptions).Member;

    [field: MaybeNull] public String Signature => field ??= symbol.ToDisplayString(SymbolDisplayFormats.Signature);
}
