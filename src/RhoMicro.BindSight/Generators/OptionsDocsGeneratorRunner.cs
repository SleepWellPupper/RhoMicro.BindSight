namespace RhoMicro.BindSight.Services;

using Generators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Runs options docs generators.
/// </summary>
/// <param name="options">
/// The options to use when running generators.
/// </param>
/// <param name="optionsProvider">
/// The the generator options provider to use when running generators.
/// </param>
/// <param name="generators">
/// The generators to run.
/// </param>
/// <param name="logger">
/// The logger to use.
/// </param>
/// <param name="hostLifetime">
/// The lifetime of the service host, to be used when exiting with
/// the <see cref="ExitMode.Host"/> mode.
/// </param>
public sealed class BindSightGeneratorRunner(
    IBindSightRunnerOptions options,
    IBindSightGeneratorOptionsProvider optionsProvider,
    IEnumerable<IBindSightGenerator> generators,
    ILogger<BindSightGeneratorRunner> logger,
    IHostApplicationLifetime hostLifetime)
{
    /// <summary>
    /// Runs the generator runner.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token used to request the running of generators to be cancelled.
    /// </param>
    /// <returns>
    /// A task, which, upon completion, will contain information on whether the generators
    /// runner actually did run. The task will complete upon all generators having ran or
    /// the runner exiting early due to <see cref="IBindSightRunnerOptions.ExitOnCancellation"/>.
    /// </returns>
    public async Task<Boolean> Run(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!options.Run)
            return false;

        var exitCode = await RunGenerators(cancellationToken);

        logger.LogInformation("Exiting with mode '{Mode}'.", options.ExitMode);

        if (cancellationToken.IsCancellationRequested && !options.ExitOnCancellation)
            return true;

        if (options.ExitMode is ExitMode.Environment)
            Environment.Exit(exitCode);
        if (options.ExitMode is ExitMode.Host)
            hostLifetime.StopApplication();

        return true;
    }

    private async ValueTask<Int32> RunGenerators(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running generators.");
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var optionsTask = optionsProvider.GetOptions(cancellationToken);
            if (!optionsTask.IsCompletedSuccessfully)
                await optionsTask;

            var options = optionsTask.Result;

            await Parallel.ForEachAsync(
                generators,
                cancellationToken,
                (g, ct) => RunGenerator(g, options, ct));
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Cancelled runner.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while running generators.");
            return -1;
        }

        logger.LogInformation("Done running generators.");
        return 0;
    }

    private async ValueTask RunGenerator(
        IBindSightGenerator optionsDocsGenerator,
        IBindSightGeneratorOptions optionsDocsGeneratorOptions,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Running generator '{Generator}'.", optionsDocsGenerator);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = optionsDocsGenerator.Run(optionsDocsGeneratorOptions, cancellationToken);
            if (!task.IsCompletedSuccessfully)
                await task;

            logger.LogInformation("Done running generator '{Generator}'.", optionsDocsGenerator);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while running generator '{Generator}'.", optionsDocsGenerator);
        }
    }
}
