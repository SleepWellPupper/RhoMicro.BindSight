namespace RhoMicro.OptionsDocs.Services;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Provides extension methods for <see cref="IHost"/>.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Runs the options docs generators registered to the hosts service collection.
    /// </summary>
    /// <param name="host">
    /// The host whose registered options docs generators to run.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token used to request the running of generators to be cancelled.
    /// </param>
    /// <returns>
    /// A task, which, upon completion, will contain information on whether the generators
    /// runner actually did run. The task will complete upon all generators having ran or
    /// the runner exiting early due to <see cref="IOptionsDocsRunnerOptions.ExitOnCancellation"/>.
    /// </returns>
    public static Task<Boolean> RunOptionsDocs(this IHost host, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = host.Services.RunOptionsDocs(cancellationToken);

        return result;
    }
}
