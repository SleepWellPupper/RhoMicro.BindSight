namespace ReferenceGenerator.XmlDocs;

using Microsoft.CodeAnalysis;

public readonly struct XmlDocsSymbolReference(XmlDocsSemanticModel semanticModel, String id)
{
    public String Id => id;
    public IXmlDocsSymbol? XmlDocsSymbol => semanticModel.GetDocumentationSymbol(id);
    public ISymbol? Symbol => semanticModel.GetSymbol(id);
}
