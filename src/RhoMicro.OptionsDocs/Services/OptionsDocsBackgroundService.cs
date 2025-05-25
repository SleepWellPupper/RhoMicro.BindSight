namespace RhoMicro.OptionsDocs.Services;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Background service that runs the injected <see cref="OptionsDocsGeneratorRunner"/> upon being executed.
/// </summary>
/// <param name="optionsDocsGeneratorRunner">
/// The docs options docs runner to run.
/// </param>
public sealed class OptionsDocsBackgroundService(OptionsDocsGeneratorRunner optionsDocsGeneratorRunner) : BackgroundService
{
    /// <inheritdoc />
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => optionsDocsGeneratorRunner.Run(stoppingToken);
}
