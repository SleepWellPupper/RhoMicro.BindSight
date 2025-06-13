namespace RhoMicro.BindSight.Models;

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Enrichments;
using HotterReload;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents an options registration.
/// </summary>
public sealed class OptionsModel : IEquatable<OptionsModel>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="root">
    /// The model of  the registered options type.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The factory used to create enrichments to the new model instance.
    /// </param>
    /// <param name="children">
    /// The models of bound object graphs defined by root properties.
    /// </param>
    public OptionsModel(
        OptionsModelRoot root,
        ImmutableHashSet<OptionsModelChild> children,
        EnrichmentFactory enrichmentFactory)
        : this(root, children, enrichmentFactory, enrichments: [])
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="root">
    /// The model of  the registered options type.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The factory used to create enrichments to the new model instance.
    /// </param>
    public OptionsModel(
        OptionsModelRoot root,
        EnrichmentFactory enrichmentFactory)
        : this(root, children: null, enrichmentFactory, enrichments: [])
    {
    }

    private OptionsModel(
        OptionsModelRoot root,
        ImmutableHashSet<OptionsModelChild>? children,
        EnrichmentFactory enrichmentFactory,
        ConcurrentDictionary<Type, Object> enrichments)
    {
        _enrichments = enrichments;
        _enrichmentFactory = enrichmentFactory;
        Root = root;
        ChildrenInternal = children!;
    }


    private readonly ConcurrentDictionary<Type, Object> _enrichments;
    private readonly EnrichmentFactory _enrichmentFactory;

    /// <summary>
    /// Gets the model of  the registered options type.
    /// </summary>
    public OptionsModelRoot Root { get; }

    private ImmutableHashSet<OptionsModelChild> ChildrenInternal
    {
        get => field ?? throw new InvalidOperationException("Children were not initialized yet.");
        set => field = field is null
            ? value
            : throw new InvalidOperationException("Children were already initialized.");
    }

    /// <summary>
    /// Gets the models of bound object graphs defined by root properties.
    /// </summary>
    public ImmutableHashSet<OptionsModelChild> Children => ChildrenInternal;

    /// <summary>
    /// Creates a copy of this instance with <see cref="Root"/> replaced.
    /// </summary>
    /// <param name="root">
    /// The value to replace <see cref="Root"/> with.
    /// </param>
    /// <returns>
    /// A copy of this instance with <see cref="Root"/> replaced by <paramref name="root"/>.
    /// </returns>
    public OptionsModel WithRoot(OptionsModelRoot root) =>
        new(root, Children, _enrichmentFactory, [.._enrichments]);

    /// <summary>
    /// Creates a copy of this instance with <see cref="Children"/> replaced.
    /// </summary>
    /// <param name="children">
    /// The value to replace <see cref="Children"/> with.
    /// </param>
    /// <returns>
    /// A copy of this instance with <see cref="Children"/> replaced by <paramref name="children"/>.
    /// </returns>
    public OptionsModel WithChildren(ImmutableHashSet<OptionsModelChild> children) =>
        new(Root, children, _enrichmentFactory, [.._enrichments]);

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
        where TEnrichment : IOptionsModelEnrichment<TEnrichment>
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
    public OptionsModel WithEnrichment<TEnrichment>(TEnrichment enrichment)
        where TEnrichment : IOptionsModelEnrichment<TEnrichment>
    {
        var enrichments = new ConcurrentDictionary<Type, Object>(_enrichments) { [typeof(TEnrichment)] = enrichment };
        var result = new OptionsModel(Root, Children, _enrichmentFactory, enrichments);

        return result;
    }

    /// <summary>
    /// Creates a new instance using default creation options.
    /// </summary>
    /// <param name="type">
    /// The options type to model.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The enrichment factory to be used by the created instance when creating enrichments.
    /// </param>
    /// <returns>
    /// A new model of the options type.
    /// </returns>
    public static OptionsModel Create(Type type, EnrichmentFactory enrichmentFactory)
        => Create(type, enrichmentFactory, OptionsModelCreationOptions.Default);

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="type">
    /// The options type to model.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The enrichment factory to be used by the created instance when creating enrichments.
    /// </param>
    /// <param name="options">
    /// The options to use when creating the model.
    /// </param>
    /// <returns>
    /// A new model of the options type.
    /// </returns>
    public static OptionsModel Create(
        Type type,
        EnrichmentFactory enrichmentFactory,
        OptionsModelCreationOptions options)
    {
        var rules = options.BindingRules.ToDictionary(r => r.Type);
        var root = new OptionsModelRoot(type, enrichmentFactory);
        var children = ImmutableHashSet.CreateBuilder<OptionsModelChild>();
        var result = new OptionsModel(root, enrichmentFactory);

        addChildren(parent: null, []);

        result.ChildrenInternal = children.ToImmutable();

        return result;

        void addChildren(OptionsModelChild? parent, HashSet<Type> handledTypes)
        {
            var type = parent?.ModelledType ?? result.Root.Type;

            if (!handledTypes.Add(type))
                return;

            if (!rules.TryGetValue(type, out var rule))
                rules[type] = rule = OptionsTypeBindingRule.CreateDefault(type);

            foreach (var property in rule.BoundPropertiesAccessor.Invoke(type))
            {
                var child = new OptionsModelChild(property, result, parent, enrichmentFactory);
                children.Add(child);
                addChildren(parent: child, handledTypes);
            }

            foreach (var arg in rule.BoundGenericArgumentsAccessor.Invoke(type))
            {
                var child = new OptionsModelChild(arg, result, parent, enrichmentFactory);
                children.Add(child);
                addChildren(parent: child, handledTypes);
            }
        }
    }

    /// <inheritdoc />
    public override String ToString() => Root.ToString();

    /// <inheritdoc />
    public override Boolean Equals(Object? obj) => Equals(obj as OptionsModel);

    /// <inheritdoc />
    public Boolean Equals(OptionsModel? other) => Root.Equals(other?.Root);

    /// <inheritdoc />
    public override Int32 GetHashCode() => Root.GetHashCode();
}

file static class Extensions
{
    public static void Add(this ConcurrentDictionary<Type, Object> enrichments, KeyValuePair<Type, Object> keyValuePair)
        => enrichments[keyValuePair.Key] = keyValuePair.Value;
}
