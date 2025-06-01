namespace RhoMicro.BindSight.Enrichments;

using Models;

/// <summary>
/// Provides extension methods for option models.
/// </summary>
public static class ModelExtensions
{
    /// <summary>
    /// Gets the set of disallowed values for the model.
    /// </summary>
    /// <param name="child">
    /// The model whose disallowed values to get.
    /// </param>
    /// <returns>
    /// The set of disallowed values.
    /// </returns>
    public static IReadOnlySet<Object?> GetDisallowedValues(this OptionsModelChild child)
        => child.GetEnrichment<ValuesEnrichment>().DisallowedValues;

    /// <summary>
    /// Gets the set of allowed values for the model.
    /// </summary>
    /// <param name="child">
    /// The model whose allowed values to get.
    /// </param>
    /// <returns>
    /// The set of allowed values.
    /// </returns>
    public static IReadOnlySet<Object?> GetAllowedValues(this OptionsModelChild child)
        => child.GetEnrichment<ValuesEnrichment>().AllowedValues;

    /// <summary>
    /// Gets the set of disallowed values for the model.
    /// </summary>
    /// <param name="root">
    /// The model whose disallowed values to get.
    /// </param>
    /// <returns>
    /// The set of disallowed values.
    /// </returns>
    public static IReadOnlySet<Object?> GetDisallowedValues(this OptionsModelRoot root)
        => root.GetEnrichment<ValuesEnrichment>().DisallowedValues;

    /// <summary>
    /// Gets the set of allowed values for the model.
    /// </summary>
    /// <param name="root">
    /// The model whose allowed values to get.
    /// </param>
    /// <returns>
    /// The set of allowed values.
    /// </returns>
    public static IReadOnlySet<Object?> GetAllowedValues(this OptionsModelRoot root)
        => root.GetEnrichment<ValuesEnrichment>().AllowedValues;

    /// <summary>
    /// Gets the description for the model.
    /// </summary>
    /// <param name="child">
    /// The model whose description to get.
    /// </param>
    /// <returns>
    /// The description.
    /// </returns>
    public static String GetDescription(this OptionsModelChild child)
        => child.GetEnrichment<DescriptionEnrichment>().Description;

    /// <summary>
    /// Gets the description for the model.
    /// </summary>
    /// <param name="root">
    /// The model whose description to get.
    /// </param>
    /// <returns>
    /// The description.
    /// </returns>
    public static String GetDescription(this OptionsModelRoot root)
        => root.GetEnrichment<DescriptionEnrichment>().Description;

    /// <summary>
    /// Gets the name for the model.
    /// </summary>
    /// <param name="child">
    /// The model whose name to get.
    /// </param>
    /// <returns>
    /// The name.
    /// </returns>
    public static String GetName(this OptionsModelChild child)
        => child.GetEnrichment<NameEnrichment>().Name;

    /// <summary>
    /// Gets the name for the model.
    /// </summary>
    /// <param name="root">
    /// The model whose name to get.
    /// </param>
    /// <returns>
    /// The name.
    /// </returns>
    public static String GetName(this OptionsModelRoot root)
        => root.GetEnrichment<NameEnrichment>().Name;

    /// <summary>
    /// Gets the key for the model.
    /// </summary>
    /// <param name="child">
    /// The model whose key to get.
    /// </param>
    /// <returns>
    /// The key.
    /// </returns>
    public static String GetKey(this OptionsModelChild child)
        => child.GetEnrichment<ChildKeyEnrichment>().Key;

    /// <summary>
    /// Gets the key for the model.
    /// </summary>
    /// <param name="root">
    /// The model whose key to get.
    /// </param>
    /// <returns>
    /// The key.
    /// </returns>
    public static String GetKey(this OptionsModelRoot root)
        => root.GetEnrichment<RootKeyEnrichment>().Key;
}
