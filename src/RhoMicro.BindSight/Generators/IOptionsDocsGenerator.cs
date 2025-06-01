namespace RhoMicro.BindSight.Generators;

/// <summary>
/// Generates documentation for options.
/// </summary>
public interface IBindSightGenerator
{
    /// <summary>
    /// Runs the generator.
    /// </summary>
    /// <param name="optionsDocsGeneratorOptions">
    /// The options to use.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token used to request generating to be cancelled.
    /// </param>
    /// <returns>
    /// A task representing the generating of documentation.
    /// </returns>
    ValueTask Run(IBindSightGeneratorOptions optionsDocsGeneratorOptions, CancellationToken cancellationToken =  default);
}
