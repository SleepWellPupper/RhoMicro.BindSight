namespace RhoMicro.OptionsDocs.Generators;

using System.ComponentModel;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for the <see cref="ReadmeGenerator"/>.
/// </summary>
[Options]
public partial interface IReadmeGeneratorOptions
{
    /// <summary>
    /// Gets the path of the readme file to generate options docs into. The default is <c>Options/Readme.md</c>.
    /// </summary>
    [Description("Sets the path of the readme file to generate options docs into. The default is `Options/Readme.md`."),
     DefaultValueExpression(@"""Options/Readme.md""")]
    String OutputFile { get; }

    /// <summary>
    /// Gets the title of the readme file. The default is <c>Options</c>.
    /// </summary>
    [Description("Sets the title of the readme file. The default is `Options`."),
     DefaultValueExpression(@"""Options""")]
    String Title { get; }
}
