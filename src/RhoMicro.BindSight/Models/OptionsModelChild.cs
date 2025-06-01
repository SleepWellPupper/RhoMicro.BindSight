namespace RhoMicro.BindSight.Models;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using Enrichments;

/// <summary>
/// Models a direct or indirect property or generic argument thereof of a registered options type.
/// </summary>
public sealed class OptionsModelChild : IEquatable<OptionsModelChild>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="property">
    /// The property to model.
    /// </param>
    /// <param name="model">
    /// The model containing the root to this property.
    /// </param>
    /// <param name="parent">
    /// The parent to this child.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The factory used to create enrichments to the new model instance.
    /// </param>
    public OptionsModelChild(
        PropertyInfo property,
        OptionsModel model,
        OptionsModelChild? parent,
        EnrichmentFactory enrichmentFactory)
        : this([], enrichmentFactory, property, genericArgumentType: null, parent, model)
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="genericArgumentType">
    /// The generic argument type to model.
    /// </param>
    /// <param name="model">
    /// The model containing the root to this property.
    /// </param>
    /// <param name="parent">
    /// The parent to this child.
    /// </param>
    /// <param name="enrichmentFactory">
    /// The factory used to create enrichments to the new model instance.
    /// </param>
    public OptionsModelChild(
        Type genericArgumentType,
        OptionsModel model,
        OptionsModelChild? parent,
        EnrichmentFactory enrichmentFactory)
        : this([], enrichmentFactory, property: null, genericArgumentType, parent, model)
    {
    }

    private readonly ConcurrentDictionary<Type, Object> _enrichments;
    private readonly EnrichmentFactory _enrichmentFactory;

    private OptionsModelChild(
        ConcurrentDictionary<Type, Object> enrichments,
        EnrichmentFactory enrichmentFactory,
        PropertyInfo? property,
        Type? genericArgumentType,
        OptionsModelChild? parent,
        OptionsModel model)
    {
        _enrichments = enrichments;
        _enrichmentFactory = enrichmentFactory;
        Parent = parent;
        Model = model;

        Property = property;
        GenericArgumentType = genericArgumentType;
    }

    /// <summary>
    /// Gets the property info modelled by this instance,
    /// if it is modelling one; otherwise, <see langword="null"/>.
    /// </summary>
    public PropertyInfo? Property { get; }

    /// <summary>
    /// Gets the generic type argument modelled by this instance,
    /// if it is modelling one; otherwise, <see langword="null"/>.
    /// </summary>
    public Type? GenericArgumentType { get; }

    /// <summary>
    /// Gets the parent containing this model if it is contained in one; otherwise, <see langword="null"/>.
    /// </summary>
    public OptionsModelChild? Parent { get; }

    /// <summary>
    /// Gets the options model containing the root defining this child.
    /// </summary>
    public OptionsModel Model { get; }

    /// <summary>
    /// Gets the type of the modelled property or the modelled type.
    /// </summary>
    public Type ModelledType => Property is not null ? Property.PropertyType : GenericArgumentType!;

    /// <summary>
    /// Gets the first found custom attribute of type <typeparamref name="TAttribute"/>
    /// on either <see cref="Property"/> or <see cref="GenericArgumentType"/>,
    /// depending on which is being modelled by this instance.
    /// </summary>
    /// <inheritdoc cref="CustomAttributeExtensions.GetCustomAttribute{T}(MemberInfo, Boolean)" path="/param" />
    /// <typeparam name="TAttribute">
    /// The type of attribute to look for.
    /// </typeparam>
    /// <returns>
    /// The first found attribute of type <typeparamref name="TAttribute"/>, if one
    /// could be found; otherwise, <see langword="null"/>.
    /// </returns>
    public TAttribute? GetFirstCustomAttribute<TAttribute>(Boolean inherit = false)
        where TAttribute : Attribute
        => GetCustomAttributes<TAttribute>(inherit) is [{ } first, ..]
            ? first
            : null;

    /// <summary>
    /// Gets all custom attribute of type <typeparamref name="TAttribute"/>
    /// on either <see cref="Property"/> or <see cref="GenericArgumentType"/>,
    /// depending on which is being modelled by this instance.
    /// </summary>
    /// <inheritdoc cref="CustomAttributeExtensions.GetCustomAttribute{T}(MemberInfo, Boolean)" path="/param" />
    /// <typeparam name="TAttribute">
    /// The type of attribute to look for.
    /// </typeparam>
    /// <returns>
    /// All attributes of type <typeparamref name="TAttribute"/>, if one
    /// could be found; otherwise, <see langword="null"/>.
    /// </returns>
    public ImmutableArray<TAttribute> GetCustomAttributes<TAttribute>(Boolean inherit = false)
        where TAttribute : Attribute =>
    [
        .. GenericArgumentType?.GetCustomAttributes<TAttribute>(inherit)
           ?? Property!.GetCustomAttributes<TAttribute>(inherit)
    ];

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
        where TEnrichment : notnull, IOptionsModelChildEnrichment<TEnrichment>
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
    public OptionsModelChild WithEnrichment<TEnrichment>(TEnrichment enrichment)
        where TEnrichment : IOptionsModelChildEnrichment<TEnrichment>
    {
        var enrichments = new ConcurrentDictionary<Type, Object>(_enrichments) { [typeof(TEnrichment)] = enrichment };
        var result = new OptionsModelChild(enrichments, _enrichmentFactory, Property, GenericArgumentType, Parent,
            Model);

        return result;
    }

    /// <inheritdoc />
    public override Int32 GetHashCode() => HashCode.Combine(Property, GenericArgumentType, Parent);

    /// <inheritdoc />
    public override Boolean Equals(Object? obj) => Equals(obj as OptionsModelChild);

    /// <inheritdoc />
    public Boolean Equals(OptionsModelChild? other)
        => other is not null &&
           other.Property == Property &&
           other.GenericArgumentType == GenericArgumentType &&
           EqualityComparer<OptionsModelChild?>.Default.Equals(Parent, other.Parent);
}
