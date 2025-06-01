namespace RhoMicro.BindSight.Generators;

using System.Text.Json.Schema;
using Microsoft.Extensions.Logging;

/// <summary>
/// Generates json schemata for option models.
/// </summary>
/// <param name="jsonSchemaOptions">
/// The options to use.
/// </param>
/// <param name="logger">
/// The logger to use.
/// </param>
public sealed class JsonSchemaGenerator(
    IJsonSchemaGeneratorOptions jsonSchemaOptions,
    ILogger<JsonSchemaGenerator> logger) : IBindSightGenerator
{
    private String GetPath(Type type)
    {
        var basePath = Path.IsPathFullyQualified(jsonSchemaOptions.OutputDirectory)
            ? jsonSchemaOptions.OutputDirectory
            : Path.Combine(Environment.CurrentDirectory, jsonSchemaOptions.OutputDirectory);

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var result = Path.Combine(basePath, $"{type.FullName}.json");

        return result;
    }

    /// <inheritdoc />
    public async ValueTask Run(IBindSightGeneratorOptions optionsDocsGeneratorOptions, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var type in optionsDocsGeneratorOptions.Options.Select(o => o.Root.Type))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var schema = jsonSchemaOptions.JsonSerializerOptions
                .GetJsonSchemaAsNode(type, jsonSchemaOptions.JsonSchemaExporterOptions)
                .ToString();
            var path = GetPath(type);
            logger.LogInformation("Writing '{Path}'.", path);
            await File.WriteAllTextAsync(path, schema, cancellationToken);
        }
    }
}
