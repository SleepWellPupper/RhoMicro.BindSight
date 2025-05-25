namespace RhoMicro.OptionsDocs.Models;

using System.Collections.Concurrent;
using Enrichments;

/// <summary>
/// Models a registered option type.
/// </summary>
public sealed class OptionsModelRoot : IEquatable<OptionsModelRoot>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="type">
    /// The modelled options type.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The enrichment factory to use when creating enrichments.
    /// </param>
    public OptionsModelRoot(Type type, EnrichmentFactory enrichmentFactory)
        : this(type, enrichmentFactory, [])
    {
    }

    private OptionsModelRoot(Type type, EnrichmentFactory enrichmentFactory,
        ConcurrentDictionary<Type, Object> enrichments)
    {
        _enrichments = enrichments;
        _enrichmentFactory = enrichmentFactory;
        Type = type;
    }

    private readonly ConcurrentDictionary<Type, Object> _enrichments;
    private readonly EnrichmentFactory _enrichmentFactory;

    /// <summary>
    /// Gets the modelled type.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets an instance of the specified enrichment type.
    /// If an instance of the enrichment type is already attached to this instance, it is returned;
    /// otherwise, a new instance of the enrichment will be created and attached to this instance.
    /// </summary>
    /// <typeparam name="TEnrichment">
    /// The type of enrichment to get.
    /// </typeparam>
    /// <returns>
    /// The instance of <typeparamref name="TEnrichment"/> attached to this instance.
    /// </returns>
    public TEnrichment GetEnrichment<TEnrichment>()
        where TEnrichment : IOptionsModelRootEnrichment<TEnrichment>
        => (TEnrichment)_enrichments.GetOrAdd(
            typeof(TEnrichment),
            static (type, @this) => @this._enrichmentFactory.CreateEnrichment<TEnrichment>(@this),
            this);

    /// <summary>
    /// Gets a copy of this instance with the specified type of enrichment replaced.
    /// </summary>
    /// <param name="enrichment">
    /// The enrichment to attach to the returned copy.
    /// </param>
    /// <typeparam name="TEnrichment">
    /// The type of enrichment to attach to the returned copy.
    /// </typeparam>
    /// <returns>
    /// A copy of this instance with the <typeparamref name="TEnrichment"/> replaced
    /// by <paramref name="enrichment"/>.
    /// </returns>
    public OptionsModelRoot WithEnrichment<TEnrichment>(TEnrichment enrichment)
        where TEnrichment : IOptionsModelRootEnrichment<TEnrichment>
    {
        var enrichments = new ConcurrentDictionary<Type, Object>(_enrichments) { [typeof(TEnrichment)] = enrichment };
        var result = new OptionsModelRoot(Type, _enrichmentFactory, enrichments);

        return result;
    }

    /// <inheritdoc />
    public override String ToString() => Type.Name;

    /// <inheritdoc />
    public Boolean Equals(OptionsModelRoot? other) => Type == other?.Type;

    /// <inheritdoc />
    public override Boolean Equals(Object? obj) => Equals(obj as OptionsModelRoot);

    /// <inheritdoc />
    public override Int32 GetHashCode() => Type.GetHashCode();
}
