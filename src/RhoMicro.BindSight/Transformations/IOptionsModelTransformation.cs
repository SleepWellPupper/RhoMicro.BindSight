namespace RhoMicro.BindSight.Transformations;

using Models;

/// <summary>
/// Transforms options models by filtering, cleaning and enriching them.
/// </summary>
public interface IOptionsModelTransformation
{
    /// <summary>
    /// Transforms or filters an options model.
    /// </summary>
    /// <param name="model">
    /// The model to transform or filter.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token used to request cancellation of the transformation.
    /// </param>
    /// <returns>
    /// A task, that, upon completion, will contain the transformed model, or
    /// <see langword="null"/> if the options are to be filtered.
    /// </returns>
    ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default);
}
