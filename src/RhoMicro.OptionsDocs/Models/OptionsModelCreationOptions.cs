namespace RhoMicro.OptionsDocs.Models;

/// <summary>
/// Provides options for creating option models.
/// </summary>
/// <param name="bindingRules">
/// The set of binding rules defining how option types are bound against configurations.
/// </param>
public sealed class OptionsModelCreationOptions(IReadOnlySet<OptionsTypeBindingRule> bindingRules)
{
    /// <summary>
    /// Gets the explicit binding rules for options types.
    /// </summary>
    public IReadOnlySet<OptionsTypeBindingRule> BindingRules { get; } = bindingRules;

    /// <summary>
    /// Gets the default options instance.
    /// </summary>
    /// <remarks>
    /// The following binding rules are provided:
    /// <list type="bullet">
    /// <item>
    /// <see cref="Type"/> : no properties or generic arguments are defined as bindable
    /// </item>
    /// </list>
    /// </remarks>
    public static OptionsModelCreationOptions Default { get; } = new(new HashSet<OptionsTypeBindingRule>()
    {
        new(
            typeof(Type),
            OptionsTypeBindingRuleDefaults.EmptyPropertyAccessor,
            OptionsTypeBindingRuleDefaults.EmptyGenericArgumentsAccessor),
    });

    /// <summary>
    /// Gets an empty options instance.
    /// </summary>
    public static OptionsModelCreationOptions Empty { get; } = new(new HashSet<OptionsTypeBindingRule>());
}
