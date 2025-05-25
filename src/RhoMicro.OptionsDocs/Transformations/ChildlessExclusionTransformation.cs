namespace RhoMicro.OptionsDocs.Transformations;

using Models;

/// <summary>
/// Excludes all models without children.
/// </summary>
public sealed class ChildlessExclusionTransformation : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = model.Children.IsEmpty ? null : model;

        return ValueTask.FromResult(result);
    }
}
