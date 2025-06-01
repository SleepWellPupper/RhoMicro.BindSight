namespace RhoMicro.BindSight.Enrichments;

using Models;

/// <summary>
/// Enriches instances of <see cref="OptionsModelChild"/> and <see cref="OptionsModelRoot"/>
/// with allowed and disallowed values.
/// </summary>
/// <param name="disallowedValues">
/// The values disallowed for the model.
/// </param>
/// <param name="allowedValues">
/// The values allowed for the model.
/// </param>
public sealed class ValuesEnrichment(
    IReadOnlySet<Object?> disallowedValues,
    IReadOnlySet<Object?> allowedValues)
    : IOptionsModelChildEnrichment<ValuesEnrichment>, IOptionsModelRootEnrichment<ValuesEnrichment>
{
    /// <summary>
    /// Gets the values disallowed for the model.
    /// </summary>
    public IReadOnlySet<Object?> DisallowedValues { get; } = disallowedValues;
    /// <summary>
    /// Gets the values allowed for the model.
    /// </summary>
    public IReadOnlySet<Object?> AllowedValues { get; } = allowedValues;

    /// <inheritdoc />
    public static ValuesEnrichment Create(IServiceProvider services, OptionsModelChild child)
        => new(new HashSet<Object?>(), new HashSet<Object?>());

    /// <inheritdoc />
    public static ValuesEnrichment Create(IServiceProvider services, OptionsModelRoot root)
        => new(new HashSet<Object?>(), new HashSet<Object?>());
}
