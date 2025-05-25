namespace RhoMicro.OptionsDocs.Generators;

using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Models;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for generators.
/// </summary>
[Options]
public partial interface IOptionsDocsGeneratorOptions
{
    /// <summary>
    /// Gets the options models for which to generate documentation.
    /// </summary>
    [DefaultValueExpression("[]")]
    ImmutableArray<OptionsModel> Options { get; }

    /// <summary>
    /// Gets the configuration providers used.
    /// </summary>
    [DefaultValueExpression("[]")]
    ImmutableArray<IConfigurationProvider> ConfigurationProviders { get; }
}
