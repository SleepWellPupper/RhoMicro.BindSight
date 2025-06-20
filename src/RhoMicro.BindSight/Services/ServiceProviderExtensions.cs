namespace RhoMicro.BindSight.Services;

using Generators;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Runs the options docs generators registered to the service provider.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider whose registered options docs generators to run.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token used to request the running of generators to be cancelled.
    /// </param>
    /// <returns>
    /// A task, which, upon completion, will contain information on whether the generators
    /// runner actually did run. The task will complete upon all generators having ran or
    /// the runner exiting early due to <see cref="Generators.IBindSightRunnerOptions.ExitOnCancellation"/>.
    /// </returns>
    public static Task<Boolean> RunBindSight(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var runner = serviceProvider.GetRequiredService<BindSightGeneratorRunner>();

        var result = runner.Run(cancellationToken);

        return result;
    }
}
