namespace RhoMicro.BindSight.Services;

using Generators;
using Microsoft.Extensions.DependencyInjection;
using Transformations;

/// <summary>
/// Wraps a service collection for registering and configuring options docs generation.
/// </summary>
public sealed class BindSightBuilder
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="services">
    /// The service collection to wrap.
    /// </param>
    public BindSightBuilder(IServiceCollection services)
    {
        Services = services;
        Generators = new(this);
        TransformationPipeline = new TransformationPipelineBuilder(this);
    }

    /// <summary>
    /// Gets the wrapped service collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets an object for adding options docs generators to the service collection.
    /// </summary>
    public GeneratorsRegistrations Generators { get; }

    /// <summary>
    /// Gets an object for adding option model transformations to the service collection.
    /// </summary>
    public TransformationPipelineBuilder TransformationPipeline { get; }

    /// <summary>
    /// Adds a hosted service to the service collection for running the options docs
    /// runner on application startup.
    /// </summary>
    /// <returns>
    /// A reference to this builder, for chaining of further method calls.
    /// </returns>
    public BindSightBuilder AddHostedService()
    {
        Services.AddHostedService<BindSightBackgroundService>();

        return this;
    }

    /// <summary>
    /// Adds default transformations and generators.
    /// </summary>
    /// <returns>
    /// A reference to this builder, for chaining of further method calls.
    /// </returns>
    public BindSightBuilder AddDefaults()
        => AddHostedService()
            .TransformationPipeline
            .UseDefaults()
            .BindSight
            .Generators
            .AddDefaults()
            .BindSight;
}
