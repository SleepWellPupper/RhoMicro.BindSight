namespace RhoMicro.OptionsDocs.Generators;

using Enrichments;
using Microsoft.Extensions.Logging;

/// <summary>
/// Logs all options to a logger.
/// </summary>
/// <param name="logger">
/// The logger to log options to.
/// </param>
public sealed class LoggingOptionsDocsGenerator(ILogger<LoggingOptionsDocsGenerator> logger) : IOptionsDocsGenerator
{
    /// <inheritdoc />
    public ValueTask Run(IOptionsDocsGeneratorOptions optionsDocsGeneratorOptions, CancellationToken cancellationToken = default)
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
