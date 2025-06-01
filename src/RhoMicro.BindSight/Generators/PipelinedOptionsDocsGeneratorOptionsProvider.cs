namespace RhoMicro.BindSight.Generators;

using System.Collections.Immutable;
using Enrichments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using Transformations;

/// <summary>
/// Provides generator options using options transformed by <see cref="TransformationPipeline"/>.
/// </summary>
/// <param name="optionsDocsGeneratorOptions">
/// The options to use.
/// </param>
/// <param name="services">
/// The list of services to extract options from.
/// </param>
/// <param name="configuration">
/// The configuration root to extract configuration providers from.
/// </param>
/// <param name="pipeline">
/// The transformation pipeline to use when transforming option models.
/// </param>
/// <param name="enrichmentFactory">
/// The enrichment factory to pass to created models.
/// </param>
public sealed class PipelinedBindSightGeneratorOptionsProvider(
    IBindSightGeneratorOptions optionsDocsGeneratorOptions,
    IEnumerable<ServiceDescriptor> services,
    IConfigurationRoot configuration,
    TransformationPipeline pipeline,
    EnrichmentFactory enrichmentFactory)
    : IBindSightGeneratorOptionsProvider
{
    /// <inheritdoc />
    public async ValueTask<IBindSightGeneratorOptions> GetOptions(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var predefined = optionsDocsGeneratorOptions.Options.Select(m => m.Root.Type).ToHashSet();

        var registeredOptions = services
            .Where(d => d.ServiceType.GenericTypeArguments.Length == 1 &&
                        d.ServiceType.GetGenericTypeDefinition() == typeof(IConfigureOptions<>) &&
                        !predefined.Contains(d.ServiceType.GenericTypeArguments[0]))
            .Select(d => d.ServiceType.GenericTypeArguments[0])
            .Distinct()
            .Select(a => OptionsModel.Create(a, enrichmentFactory))
            .Concat(optionsDocsGeneratorOptions.Options)
            .ToImmutableArray();

        var transformationTask = pipeline.TransformOptions(registeredOptions, cancellationToken);

        if (!transformationTask.IsCompletedSuccessfully)
            await transformationTask;

        var transformedOptions = transformationTask.Result;

        var orderedOptions = transformedOptions.OrderBy(o => o.Root.Type.Name);

        var result = new BindSightGeneratorOptions()
        {
            Options = [..orderedOptions], ConfigurationProviders = [..configuration.Providers]
        };

        return result;
    }
}
