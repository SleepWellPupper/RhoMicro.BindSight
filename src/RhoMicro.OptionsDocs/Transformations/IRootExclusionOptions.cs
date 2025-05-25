namespace RhoMicro.OptionsDocs.Transformations;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for <see cref="KeyReplacementTransformation"/>.
/// </summary>
[Options]
public partial interface IRootExclusionOptions
{
    /// <summary>
    /// Gets the set of option types to exclude.
    /// </summary>
    [DefaultValueExpression("global::RhoMicro.OptionsDocs.Transformations.RootExclusionDefaults.ExcludedRootTypes")]
    IReadOnlySet<Type> ExcludedRootTypes { get; }
}
