namespace RhoMicro.BindSight.Enrichments;

using Models;

/// <summary>
/// Creates enrichments.
/// </summary>
/// <param name="services">
/// The service collection to resolve enrichments instances from.
/// </param>
public sealed class EnrichmentFactory(IServiceProvider services)
{
    /// <summary>
    /// Creates an enrichment of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="model">
    /// The model to create an enrichment for.
    /// </param>
    /// <typeparam name="T">
    /// The type of enrichment to create.
    /// </typeparam>
    /// <returns>
    /// The created enrichment.
    /// </returns>
    public T CreateEnrichment<T>(OptionsModel model)
        where T : IOptionsModelEnrichment<T>
        => T.Create(services, model);

    /// <summary>
    /// Creates an enrichment of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="child">
    /// The model to create an enrichment for.
    /// </param>
    /// <typeparam name="T">
    /// The type of enrichment to create.
    /// </typeparam>
    /// <returns>
    /// The created enrichment.
    /// </returns>
    public T CreateEnrichment<T>(OptionsModelChild child)
        where T : IOptionsModelChildEnrichment<T>
        => T.Create(services, child);

    /// <summary>
    /// Creates an enrichment of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="root">
    /// The model to create an enrichment for.
    /// </param>
    /// <typeparam name="T">
    /// The type of enrichment to create.
    /// </typeparam>
    /// <returns>
    /// The created enrichment.
    /// </returns>
    public T CreateEnrichment<T>(OptionsModelRoot root)
        where T : IOptionsModelRootEnrichment<T>
        => T.Create(services, root);
}
