namespace RhoMicro.BindSight.Transformations;

using System.Reflection;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for <see cref="ChildExclusionTransformation"/>.
/// </summary>
[Options]
public partial interface IChildExclusionOptions
{
    /// <summary>
    /// Gets the set of properties to exclude.
    /// </summary>
    [DefaultValueExpression("new global::System.Collections.Generic.HashSet<global::System.Reflection.PropertyInfo>()")]
    IReadOnlySet<PropertyInfo> ExcludedProperties { get; }

    /// <summary>
    /// Gets a value indicating whether to exclude children whose modelled type is a generic type argument.
    /// </summary>
    [DefaultValueExpression("true")]
    Boolean ExcludeGenericArgumentLeafs { get; }
}
