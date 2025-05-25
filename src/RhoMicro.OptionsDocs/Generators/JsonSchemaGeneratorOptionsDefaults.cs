namespace RhoMicro.OptionsDocs.Generators;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

internal static class JsonSchemaGeneratorOptionsDefaults
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new(JsonSerializerOptions.Web)
    {
        PropertyNameCaseInsensitive = false
    };

    public static JsonSchemaExporterOptions JsonSchemaExporterOptions { get; } = new()
    {
        TreatNullObliviousAsNonNullable = true,
        TransformSchemaNode = static (ctx, node) =>
        {
            SetDescription(ctx, node);

            if(IsEnumProperty(ctx, out var enumType))
                SetEnumSchema(enumType, node);

            return node;
        }
    };

    private static void SetDescription(JsonSchemaExporterContext ctx, JsonNode node)
    {
        var description = ctx.PropertyInfo
            ?.AttributeProvider
            ?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)
            .OfType<DescriptionAttribute>()
            .SingleOrDefault()
            ?.Description;

        if(description is not null)
            node["description"] = description;
    }

    private static void SetEnumSchema(Type enumType, JsonNode node)
    {
        var names = Enum.GetNames(enumType)
            .Select(n => JsonValue.Create(n))
            .Cast<JsonNode?>();

        var values = Enum.GetValues(enumType)
            .OfType<Object>()
            .Select(v => JsonValue.Create(v))
            .Cast<JsonNode?>();

        node["enum"] = new JsonArray([..names, ..values]);
        node.AsObject()?.Remove("type");
    }

    private static Boolean IsEnumProperty(JsonSchemaExporterContext ctx, [NotNullWhen(true)] out Type? enumType)
    {
        if(ctx is { PropertyInfo.PropertyType: { IsEnum: true } t })
        {
            enumType = t;
            return true;
        }

        enumType = null;
        return false;
    }
}