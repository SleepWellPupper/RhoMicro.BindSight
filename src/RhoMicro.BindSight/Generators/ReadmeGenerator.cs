namespace RhoMicro.BindSight.Generators;

using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Enrichments;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Models;

/// <summary>
/// Generates a readme file outlining the available options.
/// </summary>
/// <param name="readmeOptions">
/// The options to use.
/// </param>
/// <param name="logger">
/// The logger to use.
/// </param>
public sealed partial class ReadmeGenerator(
    IReadmeGeneratorOptions readmeOptions,
    ILogger<ReadmeGenerator> logger) : IBindSightGenerator
{
    private String GetPath()
    {
        var result = Path.IsPathFullyQualified(readmeOptions.OutputFile)
            ? readmeOptions.OutputFile
            : Path.Combine(Environment.CurrentDirectory, readmeOptions.OutputFile);

        var directory = Path.GetDirectoryName(result) ?? String.Empty;

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        return result;
    }

    /// <inheritdoc />
    public ValueTask Run(IBindSightGeneratorOptions optionsDocsGeneratorOptions,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = GetPath();

        var builder = new MarkdownBuilder(readmeOptions.Title);

        writeProviders();
        writeOptions();

        var markdown = builder.ToString();

        logger.LogInformation("Writing to '{Path}'.", path);
        var result = File.WriteAllTextAsync(path, markdown, cancellationToken);

        return new(result);

        void writeRoot(OptionsModelRoot root)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var key = root.GetKey();

            builder.Header(3, root.GetKey());
            builder.TableHead(String.Empty, String.Empty);
            builder.TableRow("Key", $"`{key}`");
            builder.TableRow("Type", GetTypeDetails(root.Type));

            if (root.GetDescription().Length > 0)
                builder.TableRow("Description", root.GetDescription());

            if (root.GetAllowedValues().Count > 0)
            {
                builder.TableRow("Allowed Values",
                    String.Join(", ", root.GetAllowedValues().Select(GetValueExpression)));
            }

            if (root.GetDisallowedValues().Count > 0)
            {
                builder.TableRow("Disallowed Values",
                    String.Join(", ", root.GetDisallowedValues().Select(GetValueExpression)));
            }
        }

        void writeChild(OptionsModelChild child)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var key = child.GetKey();

            builder.Header(5, child.GetName());
            builder.TableHead(String.Empty, String.Empty);
            builder.TableRow("Key", $"`{key}`");
            builder.TableRow("Type", GetTypeDetails(child.ModelledType));

            if (child.GetDescription().Length > 0)
                builder.TableRow("Description", child.GetDescription());

            if (child.GetAllowedValues().Count > 0)
            {
                builder.TableRow("Allowed Values",
                    String.Join(", ", child.GetAllowedValues().Select(GetValueExpression)));
            }

            if (child.GetDisallowedValues().Count > 0)
            {
                builder.TableRow("Disallowed Values",
                    String.Join(", ", child.GetDisallowedValues().Select(GetValueExpression)));
            }

            writeSampleUse(child);
        }

        void writeSampleUse(OptionsModelChild child)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO: implement provider based samples

            // if (child.GetAllowedValues() is not { Count: > 0 } allowedValues)
            //     return;
            //
            // builder.Body.AppendLine();
            // builder.Body.AppendLine("<details><summary>Sample Use</summary>");
            //
            // builder.Body.AppendLine("</details>");
            // builder.Body.AppendLine();
        }

        void writeProviders()
        {
            cancellationToken.ThrowIfCancellationRequested();

            builder.Header(2, "Configuration Providers");

            if (optionsDocsGeneratorOptions.ConfigurationProviders is [])
            {
                builder.Body.AppendLine("No providers were found.");
                return;
            }

            builder.TableHead("Type", "Details");

            foreach (var provider in optionsDocsGeneratorOptions.ConfigurationProviders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var firstCol = GetTypeDetails(provider.GetType());
                var secondCol = String.Empty;

                switch (provider)
                {
                    case EnvironmentVariablesConfigurationProvider environment:
                        secondCol = EnvironmentPrefixPattern()
                                .Match(environment.ToString()).Groups is
                            [.., { Value: [.., '\''] }, { Value: { Length: > 0 } prefix }, { Value: ['\'', ..] }]
                            ? $"Prefix: `{prefix}`"
                            : String.Empty;
                        break;
                    case JsonConfigurationProvider json:
                        secondCol = $"Path: `{json.Source.Path ?? String.Empty}`, Optional: {json.Source.Optional}";
                        break;
                }

                builder.TableRow(firstCol, secondCol);
            }
        }

        void writeOptions()
        {
            cancellationToken.ThrowIfCancellationRequested();

            builder.Header(2, "Configurable Options");

            if (optionsDocsGeneratorOptions.Options is [])
            {
                builder.Body.AppendLine("No root options were found.");
                return;
            }

            foreach (var model in optionsDocsGeneratorOptions.Options)
            {
                cancellationToken.ThrowIfCancellationRequested();

                writeRoot(model.Root);

                builder.Header(4, "Properties");

                foreach (var child in model.Children)
                    writeChild(child);
            }
        }
    }

    [GeneratedRegex(@"(Prefix: ')(.*)(')")]
    private static partial Regex EnvironmentPrefixPattern();

    private static String GetTypeDetails(Type type)
        => NonDetailTypes.Contains(type)
            ? EscapeTypeName(type)
            : $"<details><summary>`{EscapeTypeName(type)}`{GetFlagsNote(type)}</summary>`{EscapeFullTypeName(type)}`</details>";

    private static String GetFlagsNote(Type type)
        => type.IsEnum && type.GetCustomAttribute<FlagsAttribute>() is not null
            ? " (flags)"
            : String.Empty;

    private static String GetValueExpression(Object? value)
        => value switch
        {
            null => "`null`",
            Boolean b => b ? "`true`" : "`false`",
            EnumValue e => $"""`"{e.Name}"`(`{e.Value}`)""",
            String or Char => $"""`"{value}"`""",
            Type t => $"`{EscapeFullTypeName(t)}`",
            _ => $"`{value}`"
        };

    private static String EscapeTypeName(Type type) => $"``{GetTypeName(type)}``";
    private static String EscapeFullTypeName(Type type) => $"``{GetTypeName(type, includeNamespace: true)}``";

    private static readonly HashSet<Type> NonDetailTypes =
    [
        typeof(Boolean),
        typeof(SByte),
        typeof(UInt16),
        typeof(UInt32),
        typeof(UInt64),
        typeof(Byte),
        typeof(Int16),
        typeof(Int32),
        typeof(Int64),
        typeof(Single),
        typeof(Double),
        typeof(Decimal),
        typeof(String),
        typeof(Char),
    ];

    private static String GetTypeName(Type type, Boolean includeNamespace = false)
    {
        var builder = new StringBuilder();
        appendTypeName(type);
        var name = builder.ToString();
        return name;

        void appendTypeName(Type type)
        {
            if (type == typeof(Boolean))
            {
                builder.Append("bool");
                return;
            }

            if (type == typeof(SByte))
            {
                builder.Append("sbyte");
                return;
            }

            if (type == typeof(UInt16))
            {
                builder.Append("ushort");
                return;
            }

            if (type == typeof(UInt32))
            {
                builder.Append("uint");
                return;
            }

            if (type == typeof(UInt64))
            {
                builder.Append("ulong");
                return;
            }

            if (type == typeof(Byte))
            {
                builder.Append("byte");
                return;
            }

            if (type == typeof(Int16))
            {
                builder.Append("short");
                return;
            }

            if (type == typeof(Int32))
            {
                builder.Append("int");
                return;
            }

            if (type == typeof(Int64))
            {
                builder.Append("long");
                return;
            }

            if (type == typeof(Single))
            {
                builder.Append("float");
                return;
            }

            if (type == typeof(Double))
            {
                builder.Append("double");
                return;
            }

            if (type == typeof(Decimal))
            {
                builder.Append("decimal");
                return;
            }

            if (type == typeof(String))
            {
                builder.Append("string");
                return;
            }

            if (type == typeof(Char))
            {
                builder.Append("char");
                return;
            }

            if (type.IsGenericParameter)
            {
                builder.Append(type.Name);
                return;
            }

            if (includeNamespace && type.Namespace is [_, ..] n)
                builder.Append(n).Append('.');

            if (!type.IsGenericType)
                builder.Append(type.Name);
            else
            {
                builder.Append(type.Name.Split('`')[0]);
                builder.Append('<');
                foreach (var arg in type.GenericTypeArguments)
                    appendTypeName(arg);
                builder.Append('>');
            }
        }
    }
}

file sealed class MarkdownBuilder(String title)
{
    private static readonly ImmutableArray<String> _hashes =
        [..Enumerable.Range(0, 9).Select(i => new String('#', i))];

    private static readonly ImmutableArray<String> _spaces =
        [..Enumerable.Range(0, 9).Select(i => i < 3 ? String.Empty : new String(' ', (i - 3) * 2))];

    private readonly List<(Int32 depth, String header, Int32 link)> _headers = [];
    public readonly StringBuilder Body = new();

    public MarkdownBuilder TableHead(params String[] columns)
    {
        if (columns is [])
            columns = [String.Empty];

        TableRow(columns);
        TableRow(Enumerable.Repeat("--", columns.Length));

        return this;
    }

    public MarkdownBuilder TableRow(params IEnumerable<String> columns)
    {
        Body.AppendLine($"| {String.Join(" | ", columns)} |");

        return this;
    }

    public MarkdownBuilder Header(Int32 depth, String header)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(depth, 2);

        _headers.Add((depth, header, _headers.Count + 1));
        Body.AppendLine($"{_hashes[depth]} {header} <a id=\"header_{_headers.Count}\"></a>");

        return this;
    }

    public override String ToString()
    {
        var resultBuilder = new StringBuilder();
        resultBuilder
            .AppendLine($"# {title}")
            .AppendLine("# Table of Contents");

        foreach (var (depth, header, link) in _headers)
            resultBuilder.AppendLine($"{_spaces[depth]}- [{header}](#header_{link})");

        var result = resultBuilder.AppendLine().Append(Body).ToString();

        return result;
    }
}
