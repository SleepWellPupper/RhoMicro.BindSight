namespace RhoMicro.OptionsDocs.Transformations;

using Models;

/// <summary>
/// Excludes option models based on their root type.
/// </summary>
/// <param name="options">
/// The options to use when excluding options.
/// </param>
public sealed class RootExclusionTransformation(IRootExclusionOptions options) : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = options.ExcludedRootTypes.Contains(model.Root.Type)
            ? ValueTask.FromResult<OptionsModel?>(null)
            : ValueTask.FromResult<OptionsModel?>(model);

        return result;
    }
}
