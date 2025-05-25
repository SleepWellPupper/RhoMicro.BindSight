namespace RhoMicro.OptionsDocs.Generators;

using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Schema;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for the <see cref="JsonSchemaGenerator"/>.
/// </summary>
[Options]
public partial interface IJsonSchemaGeneratorOptions
{
    /// <summary>
    /// Gets the directory to generate json schemata of options into.
    /// </summary>
    [Description("Sets the directory to generate json schemata of options into."),
     DefaultValueExpression(@"""Options/Schemata/""")]
    public String OutputDirectory { get; }

    /// <summary>
    /// Gets the options to use when serializing schemata.
    /// </summary>
    [DefaultValueExpression("JsonSchemaGeneratorOptionsDefaults.JsonSerializerOptions")]
    public JsonSerializerOptions JsonSerializerOptions { get; }

    /// <summary>
    /// Gets the options to use when creating json schemata.
    /// </summary>
    [DefaultValueExpression("JsonSchemaGeneratorOptionsDefaults.JsonSchemaExporterOptions")]
    public JsonSchemaExporterOptions JsonSchemaExporterOptions { get; }
}
