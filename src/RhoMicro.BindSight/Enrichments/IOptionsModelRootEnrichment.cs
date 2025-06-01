namespace RhoMicro.BindSight.Enrichments;

using Models;

/// <summary>
/// Represents an enrichment to an instance of <see cref="OptionsModelRoot"/>.
/// </summary>
/// <typeparam name="TSelf">
/// The implementing type (CRTP).
/// </typeparam>
public interface IOptionsModelRootEnrichment<out TSelf>
    where TSelf : IOptionsModelRootEnrichment<TSelf>
{
    /// <summary>
    /// Creates a new instance of the enrichment.
    /// </summary>
    /// <param name="services">
    /// The service collection to optionally use when creating the enrichment instance.
    /// </param>
    /// <param name="root">
    /// The model for which the enrichment is being created.
    /// </param>
    /// <returns>
    /// The new enrichment instance.
    /// </returns>
    static abstract TSelf Create(IServiceProvider services, OptionsModelRoot root);
}
