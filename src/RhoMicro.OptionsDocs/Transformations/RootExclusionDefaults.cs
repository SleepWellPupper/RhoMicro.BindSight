namespace RhoMicro.OptionsDocs.Transformations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides default values for <see cref="IRootExclusionOptions"/>.
/// </summary>
public static class RootExclusionDefaults
{
    /// <summary>
    /// Gets the default value to use for <see cref="IRootExclusionOptions.ExcludedRootTypes"/>.
    /// </summary>
    public static IReadOnlySet<Type> ExcludedRootTypes { get; } = new[]
    {
        typeof(Options).Assembly
            .GetType("Microsoft.Extensions.Options.StartupValidatorOptions"),
        typeof(AutoActivationExtensions).Assembly
            .GetType("Microsoft.Extensions.DependencyInjection.AutoActivatorOptions"),
        typeof(MetricsBuilderConsoleExtensions).Assembly
            .GetType("Microsoft.Extensions.DependencyInjection.MetricsServiceExtensions+NoOpOptions"),
        typeof(JsonConsoleFormatterOptions), typeof(ConsoleFormatterOptions), typeof(SimpleConsoleFormatterOptions),
    }.Where(t => t is not null).ToHashSet()!;
}
