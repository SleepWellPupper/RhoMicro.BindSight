namespace RhoMicro.BindSight.Enrichments;

using System.Diagnostics.CodeAnalysis;
using Models;

/// <summary>
/// Enriches instances of <see cref="OptionsModelRoot"/> with the
/// configuration key they are bound against.
/// </summary>
/// <param name="key">
/// The key against which the root is bound.
/// </param>
public sealed class RootKeyEnrichment(String key) : IOptionsModelRootEnrichment<RootKeyEnrichment>
{
    /// <summary>
    /// Gets the key against which the root is bound.
    /// </summary>
    public String Key => key;

    /// <inheritdoc />
    public static RootKeyEnrichment Create(IServiceProvider services, OptionsModelRoot root)
        => new(root.Type.Name);
}
