namespace RhoMicro.OptionsDocs.Models;

using System.Reflection;

/// <summary>
/// Provides access to the binding behavior of a given options type.
/// </summary>
/// <param name="type">
/// The type to provide binding behavior rules for.
/// </param>
/// <param name="boundPropertiesAccessor">
/// The accessor providing access tto the properties of <paramref name="type"/>
/// that are bound.
/// </param>
/// <param name="boundGenericArgumentsAccessor">
/// The accessor providing access to the generic arguments of <paramref name="type"/>
/// that are bound.
/// </param>
public sealed class OptionsTypeBindingRule(
    Type type,
    Func<Type, IReadOnlySet<PropertyInfo>> boundPropertiesAccessor,
    Func<Type, IReadOnlySet<Type>> boundGenericArgumentsAccessor)
    : IEquatable<OptionsTypeBindingRule>
{
    /// <inheritdoc />
    public Boolean Equals(OptionsTypeBindingRule? other) => other?.Type == Type;

    /// <inheritdoc />
    public override Boolean Equals(Object? obj) => Equals(obj as OptionsTypeBindingRule);

    /// <inheritdoc />
    public override Int32 GetHashCode() => Type.GetHashCode();

    /// <summary>
    /// Gets the type this rule is defining binding accessors for.
    /// </summary>
    public Type Type { get; } = type;

    /// <summary>
    /// Gets the accessor providing access to the bound properties of <see cref="Type"/>.
    /// </summary>
    public Func<Type, IReadOnlySet<PropertyInfo>> BoundPropertiesAccessor { get; } = boundPropertiesAccessor;

    /// <summary>
    /// Gets the accessor providing access to the bound generic arguments of <see cref="Type"/>.
    /// </summary>
    public Func<Type, IReadOnlySet<Type>> BoundGenericArgumentsAccessor { get; } = boundGenericArgumentsAccessor;

    /// <summary>
    /// Creates the default binding rule for a type.
    /// </summary>
    /// <param name="type">
    /// The type to create the default binding rule for.
    /// </param>
    /// <returns>
    /// The default binding rule for <paramref name="type"/>.
    /// </returns>
    public static OptionsTypeBindingRule CreateDefault(Type type)
    {
        var genericArgumentAccessor = OptionsTypeBindingRuleDefaults.GetGenericArgumentsAccessor(type);
        var boundPropertiesAccessor = OptionsTypeBindingRuleDefaults.GetPropertiesAccessor(type);

        return new OptionsTypeBindingRule(type, boundPropertiesAccessor, genericArgumentAccessor);
    }
}
