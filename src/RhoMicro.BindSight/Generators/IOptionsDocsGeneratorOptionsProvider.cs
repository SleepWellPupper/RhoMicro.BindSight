namespace RhoMicro.BindSight.Generators;

/// <summary>
/// Provides options to be used in generators.
/// </summary>
public interface IBindSightGeneratorOptionsProvider
{
    /// <summary>
    /// Gets the options to use.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token used to request options retrieval to be cancelled.
    /// </param>
    /// <returns>
    /// A task, that, upon completion, will contain the options to use.
    /// </returns>
    ValueTask<IBindSightGeneratorOptions> GetOptions(CancellationToken cancellationToken = default);
}
