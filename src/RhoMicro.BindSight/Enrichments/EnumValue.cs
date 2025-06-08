namespace RhoMicro.BindSight.Enrichments;

using System.Collections.Immutable;
using System.Reflection;
using Transformations;

/// <summary>
/// Models an enum value.
/// </summary>
/// <param name="Name">
/// The name of the enum value.
/// </param>
/// <param name="Value">
/// The numerical value of the enum value.
/// </param>
/// <param name="Description">
/// The description of the enum value.
/// </param>
public sealed record EnumValue(String Name, Object Value, String Description)
{
    /// <summary>
    /// Creates an instance for all values of the provided enum type.
    /// </summary>
    /// <param name="type">
    /// The type whose enum values to retrieve.
    /// </param>
    /// <returns>
    /// An array of all enum values found in <paramref name="type"/>
    /// if it represents an enum type; otherwise, an empty array.
    /// </returns>
    public static ImmutableArray<EnumValue> Create(Type type)
        => type.IsEnum
            ?
            [
                ..type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Select(f =>
                    {
                        var name = f.Name;
                        var value = f.GetRawConstantValue() ?? 0;
                        var description = f.GetDescription();

                        return new EnumValue(name, value, description);
                    })
            ]
            : [];
}
