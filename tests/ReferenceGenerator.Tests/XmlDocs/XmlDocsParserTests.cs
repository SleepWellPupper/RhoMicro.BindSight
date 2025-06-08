namespace ReferenceGenerator.Tests.XmlDocs;

using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Security.AccessControl;
using Microsoft.CodeAnalysis;
using ReferenceGenerator.XmlDocs;
using ReferenceGenerator.XmlDocs;
using Xunit.Sdk;
using static ReferenceGenerator.XmlDocs.SyntaxFactory;

public class XmlDocsParserTests
{
    [Fact]
    public void SimpleSummary()
    {
        // Arrange
        var expected = "Hello, World!";
        var source = TestHelpers.CompileType(
                $"""
                 /// <summary>{expected}</summary>
                 class Foo;
                 """, "Foo", out var _)
            .GetDocs();

        // Act
        var result = XmlDocsParser.Create(source).Parse(TestContext.Current.CancellationToken);

        // Assert
        var success = Assert.IsType<ParseResult.Success>(result);
        var memberChild = Assert.Single(success.MemberSyntax.Children);
        var summary = Assert.IsType<SummarySyntax>(memberChild.Child);
        var summaryChildren = Assert.IsType<NestedElementsSyntax>(summary.Child.Child);
        var summaryChild = Assert.Single(summaryChildren.Children);
        var text = Assert.IsType<TextSyntax>(summaryChild.Child);
        Assert.Equal(expected, text.Text);
    }

    private static void AssertEqual(MemberSyntax expected, MemberSyntax actual)
    {
        var actualXml = actual.ToXmlString();
        var expectedXml = expected.ToXmlString();

        try
        {
            Assert.Equal(expected, actual);
            Assert.Equal(expectedXml, actualXml);
        }
        catch (XunitException e)
        {
            TestHelpers.FailWithDiff(expectedXml, actualXml, columnWidth: 48);
        }

        TestContext.Current.TestOutputHelper?.WriteLine(actualXml);
    }

    [Fact]
    public void ComplexSummary()
    {
        // Arrange
        var source = TestHelpers.CompileType(
                """
                /// <summary>
                /// Here is some text. <c>it</c> is <see langword="true"/>
                /// that <typeparamref name="T"/> is a type parameter.
                /// </summary>
                class Foo<T>;
                """, "Foo`1", out var _)
            .GetDocs();

        // Act
        var result = XmlDocsParser.Create(source).Parse(TestContext.Current.CancellationToken);

        // Assert
        var actual = Assert.IsType<ParseResult.Success>(result).MemberSyntax;
        var expected = Member(
            "T:Foo`1",
            Summary(
                Text("\n    Here is some text. "),
                InlineCode("it"),
                Text(" is "),
                SeeLangword("true"),
                Text("\n    that "),
                Typeparamref("T"),
                Text(" is a type parameter.\n    ")
            )
        );
        AssertEqual(expected, actual);
    }

    public static TheoryData<String, String, MemberSyntax> TypeDocsData =>
        new TheoryData<String, String, MemberSyntax>(
        [
            ("""
             ///
             class Foo;
             """, "Foo", Member("T:Foo")),
            ("""
             /// <summary>Represents a generic type.</summary>
             /// <typeparam name="T">
             /// The first parameter.
             /// </typeparam>
             /// <typeparam name="S">The second parameter.</typeparam>
             class Foo<T, S>;
             """, "Foo`2",
                Member("T:Foo`2",
                Summary("Represents a generic type."),
                Typeparam("T", "\n    The first parameter.\n    "),
                Typeparam("S", "The second parameter.")))
        ]);

    [Theory]
    [MemberData(nameof(TypeDocsData))]
    public void TestParserForTypeDocs(String typeSource, String typeName, MemberSyntax expected)
    {
        // Arrange
        var source = TestHelpers.CompileType(typeSource, typeName, out _).GetDocs();

        // Act
        var result = XmlDocsParser.Create(source).Parse(TestContext.Current.CancellationToken);

        // Assert
        var actual = Assert.IsType<ParseResult.Success>(result).MemberSyntax;
        AssertEqual(expected, actual);
    }

    [Fact]
    public void ParserDoesNotTrimSyntheticCommentWhitespace()
    {
        // Arrange
        var source = TestHelpers.CompileType(
                """
                /// <summary>
                /// A sample type.
                /// </summary>
                class Foo;
                """, "Foo", out _)
            .GetDocs();

        // Act
        var result = XmlDocsParser.Create(
                source,
                XmlDocsParserOptions.Default with { TrimSyntheticWhitespace = false })
            .Parse(TestContext.Current.CancellationToken);

        // Assert
        var actual = Assert.IsType<ParseResult.MemberResult>(result).Member;
        var expected = Member("Foo", Summary("\n    A sample type.\n    "));
        AssertEqual(expected, actual);
    }
}

file static class Extensions
{
    public static String GetDocs(this ISymbol symbol)
        => symbol.GetDocumentationCommentXml(
               preferredCulture: CultureInfo.InvariantCulture,
               expandIncludes: true,
               cancellationToken: TestContext.Current.CancellationToken)
           ?? String.Empty;
}
