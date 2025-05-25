namespace RhoMicro.OptionsDocs.Services;

using System.ComponentModel;

/// <summary>
/// Provides instructions to the <see cref="OptionsDocsGeneratorRunner"/> on how
/// to exit after running options docs generators.
/// </summary>
public enum ExitMode
{
    /// <summary>
    /// The runner will not exit.
    /// </summary>
    [Description("The runner will not exit.")]
    None,

    /// <summary>
    /// The runner will exit via <see cref="Microsoft.Extensions.Hosting.IHostApplicationLifetime"/>.<see cref="Microsoft.Extensions.Hosting.IHostApplicationLifetime.StopApplication"/>.
    /// </summary>
    [Description("The runner will exit via `IHostApplicationLifetime.StopApplication`.")]
    Host,

    /// <summary>
    /// The runner will exit via <see cref="Environment.Exit(int)"/>.
    /// </summary>
    [Description("The runner will exit via 'Environment.Exit(int)'.")]
    Environment
}
