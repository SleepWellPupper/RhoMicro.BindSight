namespace RhoMicro.OptionsDocs.Enrichments;

using Models;

/// <summary>
/// Enriches instances of <see cref="OptionsModelRoot"/> or <see cref="OptionsModelChild"/>
/// with names.
/// </summary>
/// <param name="name">
/// The name to attach to the model.
/// </param>
public sealed class NameEnrichment(String name)
    : IOptionsModelRootEnrichment<NameEnrichment>, IOptionsModelChildEnrichment<NameEnrichment>
{
    /// <summary>
    /// Gets the attached name.
    /// </summary>
    public String Name { get; } = name;

    /// <inheritdoc />
    public static NameEnrichment Create(IServiceProvider services, OptionsModelRoot root)
        => new(root.Type.Name);

    /// <inheritdoc />
    public static NameEnrichment Create(IServiceProvider services, OptionsModelChild child)
        => new(child.Property?.Name ?? child.GenericArgumentType!.Name);
}
