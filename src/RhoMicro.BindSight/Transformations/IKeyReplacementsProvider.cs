namespace RhoMicro.BindSight.Transformations;

/// <summary>
/// Provides replacements for <see cref="KeyReplacementTransformation"/>.
/// </summary>
public interface IKeyReplacementsProvider
{
    /// <summary>
    /// Gets the replacements to use.
    /// </summary>
    /// <returns>
    /// The replacements to use.
    /// </returns>
    IReadOnlyDictionary<String, String> GetReplacements();
}
