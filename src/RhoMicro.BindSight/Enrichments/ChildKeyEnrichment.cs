namespace RhoMicro.BindSight.Enrichments;

using System.Diagnostics.CodeAnalysis;
using Models;

/// <summary>
/// Enriches instances of <see cref="OptionsModelChild"/> with the configuration
/// keys they are bound against.
/// </summary>
/// <param name="child">
/// The enriched child.
/// </param>
/// <param name="key">
/// The key against which the child is bound.
/// </param>
/// <param name="combineWithParent">
/// Indicates whether to combine <paramref name="key"/> with the key against
/// which <paramref name="child"/>.<see cref="OptionsModelChild.Parent"/>
/// is bound.
/// </param>
public sealed class ChildKeyEnrichment(
    OptionsModelChild child,
    String key,
    Boolean combineWithParent)
    : IOptionsModelChildEnrichment<ChildKeyEnrichment>
{
    /// <summary>
    /// Gets the key of the child.
    /// </summary>
    public String Key
    {
        get
        {
            if (!combineWithParent)
                return key;

            if (child.Parent is not { } parent)
                return $"{child.Model.Root.GetKey()}:{key}";

            if (parent.Parent is
                {
                    Property.PropertyType:
                    {
                        IsGenericType: true,
                        GenericTypeArguments: [{ } elementType]
                    } grandParentPropertyType
                } grandParent &&
                OptionsTypeBindingRuleDefaults.ListTypes.Contains(grandParentPropertyType.GetGenericTypeDefinition()))
            {
                return $"{grandParent.GetKey()}:[n]:{key}";
            }

            return $"{parent.GetKey()}:{key}";
        }
    }

    /// <inheritdoc />
    public static ChildKeyEnrichment Create(IServiceProvider services, OptionsModelChild child)
        => new(child, child.Property?.Name ?? child.GenericArgumentType!.Name, combineWithParent: true);
}
