namespace RhoMicro.BindSight.Transformations;

/// <summary>
/// Provides transformations for use in <see cref="TransformationPipeline"/>.
/// </summary>
public interface ITransformationsProvider
{
    /// <summary>
    /// Gets the transformations to use.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to optionally use when creating transformations.
    /// </param>
    /// <returns>
    /// The sequence of transformations to apply.
    /// </returns>
    IEnumerable<IOptionsModelTransformation> GetTransformations(IServiceProvider serviceProvider);
}
