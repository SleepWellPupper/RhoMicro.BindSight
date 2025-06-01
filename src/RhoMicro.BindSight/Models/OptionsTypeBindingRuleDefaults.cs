namespace RhoMicro.BindSight.Models;

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// Provides binding rule defaults.
/// </summary>
public static class OptionsTypeBindingRuleDefaults
{
    private static readonly IReadOnlySet<Type> s_emptyTypes = new HashSet<Type>();
    private static readonly IReadOnlySet<PropertyInfo> s_emptyPropertyInfos = new HashSet<PropertyInfo>();

    /// <summary>
    /// Gets a generic argument accessor that will always return an empty set.
    /// </summary>
    public static Func<Type, IReadOnlySet<Type>> EmptyGenericArgumentsAccessor { get; } =
        static _ => s_emptyTypes;

    /// <summary>
    /// Gets a property accessor that will always return an empty set.
    /// </summary>
    public static Func<Type, IReadOnlySet<PropertyInfo>> EmptyPropertyAccessor { get; } =
        static _ => s_emptyPropertyInfos;

    /// <summary>
    /// Gets a generic argument accessor that retrieves the single type argument
    /// of a type, or an empty set if it has no type arguments or more than one
    /// argument.
    /// </summary>
    public static Func<Type, IReadOnlySet<Type>> ItemTypeAccessor { get; } =
        static t => t.GenericTypeArguments is [{ } itemType] ? new HashSet<Type> { itemType } : [];

    /// <summary>
    /// Gets a generic argument accessor that retrieves the second type argument
    /// of a type, or an empty set if it has more or less than two type arguments.
    /// </summary>
    public static Func<Type, IReadOnlySet<Type>> ValueTypeAccessor { get; } =
        static t => t.GenericTypeArguments is [_, { } valueType] ? new HashSet<Type> { valueType } : [];

    /// <summary>
    /// Gets the default property accessor that yields all bindable properties of a type.
    /// </summary>
    public static Func<Type, IReadOnlySet<PropertyInfo>> PropertiesAccessor { get; } =
        static t => t.GetProperties().Where(IsBindable).ToHashSet();

    /// <summary>
    /// Gets the set of bindable primitive types.
    /// </summary>
    public static ImmutableHashSet<Type> Primitives { get; } =
    [
        typeof(Boolean), typeof(Byte), typeof(Char), typeof(Decimal), typeof(Double), typeof(Single),
        typeof(Int32), typeof(Int64), typeof(SByte), typeof(Int16), typeof(UInt32),
        typeof(UInt64), typeof(UInt16), typeof(String),
    ];

    /// <summary>
    /// Gets the set of bindable list-like types.
    /// </summary>
    [field: MaybeNull]
    public static ImmutableHashSet<Type> ListTypes =>
        field ??= MutableListTypes.Concat(ImmutableListTypes).ToImmutableHashSet();

    /// <summary>
    /// Gets the set of mutable bindable list-like types.
    /// </summary>
    public static ImmutableHashSet<Type> MutableListTypes { get; } =
    [
        typeof(List<>), typeof(HashSet<>), typeof(ICollection<>), typeof(ISet<>), typeof(IList<>)
    ];

    /// <summary>
    /// Gets the set of immutable bindable list-like types.
    /// </summary>
    public static ImmutableHashSet<Type> ImmutableListTypes { get; } =
    [
        typeof(IEnumerable<>), typeof(IReadOnlyCollection<>), typeof(IReadOnlySet<>), typeof(IReadOnlyList<>)
    ];

    /// <summary>
    /// Gets the set of bindable dictionary-like types.
    /// </summary>
    [field: MaybeNull]
    public static ImmutableHashSet<Type> DictionaryTypes =>
        field ??= MutableDictionaryTypes.Concat(ImmutableDictionaryTypes).ToImmutableHashSet();

    /// <summary>
    /// Gets the set of mutable bindable dictionary-like types.
    /// </summary>
    [field: MaybeNull]
    public static ImmutableHashSet<Type> MutableDictionaryTypes { get; } =
    [
        typeof(Dictionary<,>), typeof(IDictionary<,>)
    ];

    /// <summary>
    /// Gets the set of immutable bindable dictionary-like types.
    /// </summary>
    [field: MaybeNull]
    public static ImmutableHashSet<Type> ImmutableDictionaryTypes { get; } =
    [
        typeof(IReadOnlyDictionary<,>)
    ];

    /// <summary>
    /// Gets a value indicating whether a given type is a dictionary-like type.
    /// </summary>
    /// <param name="type">
    /// The type to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="type"/> is a dictionary-like type;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsDictionaryType(Type type)
        => type.IsGenericType && DictionaryTypes.Contains(type.GetGenericTypeDefinition());

    /// <summary>
    /// Gets a value indicating whether a given type is a list-like type.
    /// </summary>
    /// <param name="type">
    /// The type to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="type"/> is a list-like type;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsListType(Type type)
        => type.IsGenericType && ListTypes.Contains(type.GetGenericTypeDefinition());

    /// <summary>
    /// Gets the default generic arguments accessor for the given type.
    /// </summary>
    /// <param name="type">
    /// The type to get an accessor for.
    /// </param>
    /// <returns>
    /// The default generic arguments accessor for <paramref name="type"/>.
    /// </returns>
    public static Func<Type, IReadOnlySet<Type>> GetGenericArgumentsAccessor(Type type)
    {
        if (!type.IsGenericType)
            return EmptyGenericArgumentsAccessor;

        if (IsListType(type))
            return ItemTypeAccessor;

        if (IsDictionaryType(type))
            return ValueTypeAccessor;

        return EmptyGenericArgumentsAccessor;
    }

    /// <summary>
    /// Gets the default properties accessor for the given type.
    /// </summary>
    /// <param name="type">
    /// The type to get an accessor for.
    /// </param>
    /// <returns>
    /// The default properties accessor for <paramref name="type"/>.
    /// </returns>
    public static Func<Type, IReadOnlySet<PropertyInfo>> GetPropertiesAccessor(Type type)
    {
        if (!type.IsGenericType)
            return PropertiesAccessor;

        if (IsListType(type) || IsDictionaryType(type))
            return EmptyPropertyAccessor;

        return PropertiesAccessor;
    }

    private static Boolean IsBindable(PropertyInfo p) => IsBindable(p, []);

    private static Boolean IsBindable(PropertyInfo p, HashSet<Type> handledTypes)
    {
        if (!handledTypes.Add(p.PropertyType))
            return true;

        if (p.GetMethod is not { IsPublic: true })
            return false;

        if (IsBindablePrimitiveType(p.PropertyType))
            return p.SetMethod is { IsPublic: true };

        if (IsBindableMutableCollectionType(
                p.PropertyType,
                handledTypes,
                out Boolean bindableMutableCollectionElementAndKeyType))
            return bindableMutableCollectionElementAndKeyType;

        if (IsBindableImmutableCollectionType(
                p.PropertyType,
                handledTypes,
                out Boolean bindableImmutableCollectionElementAndKeyType))
            return bindableImmutableCollectionElementAndKeyType && p.SetMethod is { IsPublic: true };

        if (IsBclType(p.PropertyType))
            return false;

        if (HasBindableOrNoProperties(p.PropertyType, handledTypes))
            return p.SetMethod is { IsPublic: true };

        return false;
    }

    private static Boolean IsBclType(Type type) => type.Assembly == typeof(Int32).Assembly;

    private static Boolean IsBindable(Type type, HashSet<Type> handledTypes)
    {
        if (IsBindablePrimitiveType(type))
            return true;

        if (IsBindableMutableCollectionType(
                type,
                handledTypes,
                out Boolean bindableMutableCollectionElementAndKeyType) &&
            bindableMutableCollectionElementAndKeyType)
        {
            return true;
        }

        if (IsBindableImmutableCollectionType(
                type,
                handledTypes,
                out Boolean bindableImmutableCollectionElementAndKeyType) &&
            bindableImmutableCollectionElementAndKeyType)
        {
            return true;
        }

        if (IsBclType(type))
            return false;

        if (HasBindableOrNoProperties(type, handledTypes))
            return true;

        return false;
    }

    private static Boolean HasBindableOrNoProperties(Type type, HashSet<Type> handledTypes)
    {
        PropertyInfo[] properties = type.GetProperties();

        if (properties.Length is 0)
            return true;

        foreach (var property in properties)
        {
            if (handledTypes.Contains(property.PropertyType))
                continue;

            if (IsBindable(property, handledTypes))
                return true;
        }

        return false;
    }

    private static Boolean IsBindablePrimitiveType(Type type)
    {
        if (Primitives.Contains(type) || type.IsEnum)
            return true;

        return false;
    }

    private static Boolean IsBindableImmutableCollectionType(
        Type type,
        HashSet<Type> handledTypes,
        out Boolean bindableElementAndKeyType)
    {
        switch (type)
        {
            case { IsArray: true }:
                bindableElementAndKeyType = IsBindable(type.GetElementType()!, handledTypes);
                return true;
            case { GenericTypeArguments: [{ } itemType] }
                when (ImmutableListTypes.Contains(type.GetGenericTypeDefinition())):
                bindableElementAndKeyType = IsBindable(itemType, handledTypes);
                return true;
            case { GenericTypeArguments: [{ } keyType, { } valueType] }
                when (ImmutableDictionaryTypes.Contains(type.GetGenericTypeDefinition()))
                :
                bindableElementAndKeyType = keyType == typeof(String) && IsBindable(valueType, handledTypes);
                return true;
            default:
                bindableElementAndKeyType = false;
                return false;
        }
    }

    private static Boolean IsBindableMutableCollectionType(
        Type type,
        HashSet<Type> handledTypes,
        out Boolean bindableElementAndKeyType)
    {
        switch (type)
        {
            case { GenericTypeArguments: [{ } itemType] }
                when (MutableListTypes.Contains(type.GetGenericTypeDefinition())):
                bindableElementAndKeyType = IsBindable(itemType, handledTypes);
                return true;
            case { GenericTypeArguments: [{ } keyType, { } valueType] }
                when (MutableDictionaryTypes.Contains(type.GetGenericTypeDefinition())):
                bindableElementAndKeyType = keyType == typeof(String) && IsBindable(valueType, handledTypes);
                return true;
            default:
                bindableElementAndKeyType = false;
                return false;
        }
    }
}
