namespace RhoMicro.OptionsDocs.Enrichments;

using System.ComponentModel;
using System.Reflection;
using Models;

/// <summary>
/// Enriches instances of <see cref="OptionsModelRoot"/> or <see cref="OptionsModelChild"/>
/// with descriptions.
/// </summary>
/// <param name="description">
/// The description to attach to the model.
/// </param>
public sealed class DescriptionEnrichment(
    String description)
    : IOptionsModelRootEnrichment<DescriptionEnrichment>, IOptionsModelChildEnrichment<DescriptionEnrichment>
{
    /// <summary>
    /// Gets the attached description.
    /// </summary>
    public String Description { get; } = description;

    /// <inheritdoc />
    public static DescriptionEnrichment Create(IServiceProvider _, OptionsModelRoot root)
        => new(root.Type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? String.Empty);

    /// <inheritdoc />
    public static DescriptionEnrichment Create(IServiceProvider _, OptionsModelChild child)
        => new(child.GetFirstCustomAttribute<DescriptionAttribute>()?.Description ?? String.Empty);
}
