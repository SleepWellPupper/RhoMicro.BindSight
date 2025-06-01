namespace RhoMicro.BindSight.Generators;

using Enrichments;
using Microsoft.Extensions.Logging;

/// <summary>
/// Logs all options to a logger.
/// </summary>
/// <param name="logger">
/// The logger to log options to.
/// </param>
public sealed class LoggingBindSightGenerator(ILogger<LoggingBindSightGenerator> logger) : IBindSightGenerator
{
    /// <inheritdoc />
    public ValueTask Run(IBindSightGeneratorOptions optionsDocsGeneratorOptions, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var optionsType in optionsDocsGeneratorOptions.Options)
        {
            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation(
                "Detected options type '{Type}' in section '{Section}'.",
                optionsType.Root.Type,
                optionsType.Root.GetKey());
        }

        return ValueTask.CompletedTask;
    }
}
