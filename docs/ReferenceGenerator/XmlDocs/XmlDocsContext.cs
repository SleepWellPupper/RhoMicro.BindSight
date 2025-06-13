namespace ReferenceGenerator.XmlDocs;

using Microsoft.CodeAnalysis;

internal sealed class XmlDocsContext
{
    private XmlDocsContext(XmlDocsSemanticModel semanticModel, XmlDocsProvider provider)
    {
        Provider = provider;
        SemanticModel = semanticModel;
    }

    public XmlDocsProvider Provider { get; }
    public XmlDocsSemanticModel SemanticModel { get; }

    public static XmlDocsContext Create(
        Compilation compilation,
        CancellationToken ct)
        => Create(compilation, XmlDocsProviderOptions.Default, ct);

    public static XmlDocsContext Create(
        Compilation compilation,
        XmlDocsProviderOptions options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var semanticModel = XmlDocsSemanticModel.Create(compilation, ct);
        var provider = XmlDocsProvider.Create(semanticModel, options, ct);
        var result = new XmlDocsContext(semanticModel, provider);

        return result;
    }
}
