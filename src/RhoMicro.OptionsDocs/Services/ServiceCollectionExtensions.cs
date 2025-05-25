namespace RhoMicro.OptionsDocs.Services;

using Enrichments;
using Generators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Transformations;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds options docs services to the service collection.
    /// </summary>
    /// <param name="services">
    /// The service collection to add options docs services to.
    /// </param>
    /// <returns>
    /// A builder for configuring options docs services.
    /// </returns>
    public static OptionsDocsBuilder AddOptionsDocs(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var result = new OptionsDocsBuilder(services);

        services
            .TryAddOptionsDocsGeneratorOptions(c => c.UseCustomOptions(_ => new OptionsDocsGeneratorOptions()))
            .TryAddOptionsDocsRunnerOptions()
            .AddLogging();

        services.TryAddSingleton<EnrichmentFactory>();
        services.TryAddSingleton<ITransformationsProvider>(result.TransformationPipeline);
        services.TryAddSingleton<TransformationPipelineFactory>();
        services.TryAddSingleton(sp => sp.GetRequiredService<TransformationPipelineFactory>().Create());
        services.TryAddSingleton<OptionsDocsGeneratorRunner>();

        services.TryAddSingleton<IHostApplicationLifetime, NullApplicationHostLifetime>();
        services.TryAddSingleton<IOptionsDocsGeneratorOptionsProvider>(sp =>
            new PipelinedOptionsDocsGeneratorOptionsProvider(
                sp.GetRequiredService<IOptionsDocsGeneratorOptions>(),
                services,
                sp.GetService<IConfigurationRoot>() is { } r
                    ? r
                    : sp.GetService<IConfiguration>() is IConfigurationRoot c
                        ? c
                        : sp.GetService<ConfigurationManager>() is IConfigurationRoot m
                            ? m
                            : new ConfigurationManager(),
                sp.GetRequiredService<TransformationPipeline>(),
                sp.GetRequiredService<EnrichmentFactory>()));

        return result;
    }


    /// <summary>
    /// Gets an options builder that forwards Configure calls for the
    /// same <typeparamref name="TOptions"/> to the underlying service
    /// collection. Documentation services for the options are
    /// registered as well.
    /// </summary>
    /// <typeparam name="TOptions">
    /// The options type to be configured.
    /// </typeparam>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to.
    /// </param>
    /// <param name="configSectionPath">
    /// The name of the configuration section to bind from.
    /// </param>
    /// <param name="configureBinder">
    /// Optional. Used to configure the <see cref="BinderOptions"/>.
    /// </param>
    /// <returns>
    /// The <see cref="OptionsBuilder{TOptions}"/> so that configure calls can be chained in it.
    /// </returns>
    public static OptionsBuilder<TOptions> AddDocumentedOptions<TOptions>(
        this IServiceCollection services,
        String configSectionPath,
        Action<BinderOptions>? configureBinder = null)
        where TOptions : class
        => services.AddOptionsDocs()
            .AddDefaults()
            .TransformationPipeline.KeyReplacements.Use<TOptions>(configSectionPath)
            .OptionsDocs.Services.AddOptions<TOptions>()
            .BindConfiguration(configSectionPath, configureBinder);

    /// <summary>
    /// Gets an options builder that forwards Configure calls for the
    /// same <typeparamref name="TOptions"/> to the underlying service
    /// collection. Documentation services for the options are
    /// registered as well.
    /// </summary>
    /// <typeparam name="TOptions">
    /// The options type to be configured.
    /// </typeparam>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to.
    /// </param>
    /// <param name="name">
    /// The name of the options instance.
    /// </param>
    /// <param name="configSectionPath">
    /// The name of the configuration section to bind from.
    /// </param>
    /// <param name="configureBinder">
    /// Optional. Used to configure the <see cref="BinderOptions"/>.
    /// </param>
    /// <returns>
    /// The <see cref="OptionsBuilder{TOptions}"/> so that configure calls can be chained in it.
    /// </returns>
    public static OptionsBuilder<TOptions> AddDocumentedOptions<TOptions>(
        this IServiceCollection services,
        String name,
        String configSectionPath,
        Action<BinderOptions>? configureBinder = null)
        where TOptions : class
        => services
            .AddOptionsDocs()
            .AddDefaults()
            .TransformationPipeline.KeyReplacements.Use<TOptions>(configSectionPath)
            .OptionsDocs.Services.AddOptions<TOptions>(name)
            .BindConfiguration(configSectionPath, configureBinder);
}
