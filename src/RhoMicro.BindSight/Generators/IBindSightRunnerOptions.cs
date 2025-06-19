namespace RhoMicro.BindSight.Generators;

using System.ComponentModel;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Provides options for the <see cref="BindSightGeneratorRunner"/>.
/// </summary>
[Description("Provides options for the options docs runner."), Options]
public partial interface IBindSightRunnerOptions
{
    /// <summary>
    /// Gets the exit mode of the runner. The default is <see cref="ExitMode.Environment"/>
    /// </summary>
    [Description(
         "Sets the exit mode after running all options docs generators. The default is `ExitMode.Environment`."),
     DefaultValueExpression("global::RhoMicro.BindSight.Generators.ExitMode.Environment")]
    ExitMode ExitMode { get; }

    /// <summary>
    /// Gets a value indicating whether to exit using <see cref="ExitMode"/> when cancellation is requested.
    /// The default is <see langword="true"/>.
    /// </summary>
    [Description(
         "If set to `true`, the options docs runner will exit after receiving cancellation. The default is `true`."),
     DefaultValueExpression("true")]
    Boolean ExitOnCancellation { get; }

    /// <summary>
    /// Gets a value indicating whether to run the runner. This is useful when executing the runner from
    /// a hosted service, as it may be toggled on or off using this option. The default is <see langword="true"/>.
    /// </summary>
    [Description("If set to `true`, the options docs runner will run. The default is `false`."),
     DefaultValueExpression("false")]
    Boolean Run { get; }
}
