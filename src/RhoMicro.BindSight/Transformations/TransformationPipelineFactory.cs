namespace RhoMicro.BindSight.Transformations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// Creates instances of <see cref="TransformationPipeline"/>.
/// </summary>
/// <param name="transformationsProvider">
/// The transformations provider to use.
/// </param>
/// <param name="serviceProvider">
/// The service provider to use.
/// </param>
public sealed class TransformationPipelineFactory(
    ITransformationsProvider transformationsProvider,
    IServiceProvider serviceProvider)
{
    /// <summary>
    /// Creates a new pipeline.
    /// </summary>
    /// <returns>
    /// A new pipeline instance.
    /// </returns>
    public TransformationPipeline Create() => new(
        transformationsProvider.GetTransformations(serviceProvider),
        serviceProvider.GetRequiredService<ILogger<TransformationPipeline>>());
}
