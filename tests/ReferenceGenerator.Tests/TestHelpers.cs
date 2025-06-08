namespace ReferenceGenerator.Tests;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Basic.Reference.Assemblies;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal static partial class TestHelpers
{
    public static Compilation Compile(String source)
    {
        var tree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(LanguageVersion.Preview),
            cancellationToken: TestContext.Current.CancellationToken);
        var compilation = CSharpCompilation.Create(
            TestContext.Current.Test?.TestDisplayName ?? "AnonymousTestAssembly",
            [tree],
            Net90.References.All,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return compilation;
    }

    public static IAssemblySymbol CompileAssembly(
        String source,
        out Compilation compilation)
    {
        compilation = Compile(source);
        var result = compilation.Assembly;

        return result;
    }

    public static INamedTypeSymbol CompileType(
        String source,
        String fullyQualifiedMetadataName,
        out Compilation compilation)
    {
        var assembly = CompileAssembly(source, out compilation);
        var type = assembly.GetTypeByMetadataName(fullyQualifiedMetadataName);

        if (type is null)
            Assert.Fail($"Unable to locate type from metadata name '{fullyQualifiedMetadataName}'.");

        return type;
    }

    public static TMember CompileTypeMember<TMember>(
        String source,
        String containingTypeFullyQualifiedMetadataName,
        String memberName,
        out Compilation compilation)
        where TMember : ISymbol
    {
        var type = CompileType(source, containingTypeFullyQualifiedMetadataName, out compilation);
        var member = type.GetMembers(memberName).OfType<TMember>().SingleOrDefault();

        if (member is null)
            Assert.Fail($"Unable to locate member of type '{typeof(TMember).Name}' with name '{memberName}'.");

        return member;
    }

    public static void FailWithDiff(String expected, String? actual, Int32 columnWidth = 96)
    {
        var diff = GetDiff(expected, actual, columnWidth);

        Assert.Fail(diff);
    }

    [GeneratedRegex(@"(\r\n)|(\r)|(\n)", RegexOptions.Compiled)]
    private static partial Regex NewlinePattern();

    private static String VisualizeNewlines(String text) =>
        NewlinePattern()
            .Replace(text, static m => m.ValueSpan switch
            {
                ['\r'] => "\\r\n",
                ['\n'] => "\\n\n",
                ['\r', '\n'] => "\\r\\n\n",
                _ => throw new InvalidOperationException("unexpected match")
            });

    public static String GetDiff(String expected, String? actual, Int32 columnWidth)
    {
        var diff = SideBySideDiffBuilder.Diff(
            VisualizeNewlines(expected),
            VisualizeNewlines(actual ?? String.Empty),
            ignoreCase: false,
            ignoreWhiteSpace: false);

        var sb = new StringBuilder();

        // Add headers
        _ = sb.AppendLine(
            CultureInfo.InvariantCulture,
            $"{"expected".PadRight(columnWidth)} | {"actual".PadRight(columnWidth)}");
        _ = sb.AppendLine(new String('-', columnWidth * 2 + 10)); // Divider line

        // Determine the maximum number of lines to process
        var maxLines = Math.Max(diff.OldText.Lines.Count, diff.NewText.Lines.Count);

        for (var i = 0; i < maxLines; i++)
        {
            // Fetch lines from old and new texts
            var oldLine = i < diff.OldText.Lines.Count ? diff.OldText.Lines[i] : null;
            var newLine = i < diff.NewText.Lines.Count ? diff.NewText.Lines[i] : null;

            // Generate the side-by-side display for each line
            var oldText = getFormattedText(oldLine, columnWidth);
            var newText = getFormattedText(newLine, columnWidth);
            var status = getStatusSymbol(newLine);

            _ = sb.AppendLine(CultureInfo.InvariantCulture, $"{oldText} | {status} {newText}");
        }

        static String getFormattedText(DiffPiece? line, Int32 columnWidth)
        {
            if (line?.Text == null)
                return "".PadRight(columnWidth); // Blank space for missing lines

            // Return the line's text truncated or padded to fit the column width
            return line.Text.PadRight(columnWidth)[..columnWidth];
        }

        static String getStatusSymbol(DiffPiece? newLine) => newLine?.Type switch
        {
            ChangeType.Inserted => "+",
            ChangeType.Modified => "~",
            ChangeType.Deleted or ChangeType.Imaginary or null => "-",
            ChangeType.Unchanged or _ => " "
        };

        return sb.ToString();
    }
}
