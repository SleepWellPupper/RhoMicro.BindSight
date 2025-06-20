namespace RhoMicro.BindSight.Services;

using Generators;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Background service that runs the injected <see cref="BindSightGeneratorRunner"/> upon being executed.
/// </summary>
/// <param name="optionsDocsGeneratorRunner">
/// The docs options docs runner to run.
/// </param>
public sealed class BindSightBackgroundService(BindSightGeneratorRunner optionsDocsGeneratorRunner) : BackgroundService
{
    /// <inheritdoc />
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => optionsDocsGeneratorRunner.Run(stoppingToken);
}
