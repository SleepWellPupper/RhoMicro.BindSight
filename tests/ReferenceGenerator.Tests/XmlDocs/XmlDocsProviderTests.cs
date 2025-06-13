namespace ReferenceGenerator.Tests.XmlDocs;

using Microsoft.CodeAnalysis;
using ReferenceGenerator.XmlDocs;
using Xunit.Sdk;
using static ReferenceGenerator.XmlDocs.DocsFactory;

public class XmlDocsProviderTests
{
    [Fact]
    public void SimpleSummary()
    {
        // Arrange
        var type = TestHelpers.CompileType(
            $"""
             /// <summary>
             /// Hello, World!
             /// </summary>
             class Foo;
             """, "Foo", out var compilation);
        var semanticModel = XmlDocsSemanticModel.Create(compilation, TestContext.Current.CancellationToken);

        // Act
        var actual = XmlDocsProvider
            .Create(semanticModel, TestContext.Current.CancellationToken)
            .GetMemberXmlDocs(type);

        // Assert
        var expected = Member("Hello, World!");
        AssertEqual(expected, actual);
    }

    private static void AssertEqual(MemberElement expected, MemberElement actual)
    {
        var actualXml = actual.ToString<XmlStringVisitor>();
        var expectedXml = expected.ToString<XmlStringVisitor>();

        try
        {
            Assert.Equal(expected, actual);
            Assert.Equal(expectedXml, actualXml);
        }
        catch (XunitException)
        {
            TestHelpers.FailWithDiff(expectedXml, actualXml, columnWidth: 48);
        }

        TestContext.Current.TestOutputHelper?.WriteLine(actualXml);
    }

    [Fact]
    public void ComplexSummary()
    {
        // Arrange
        var type = TestHelpers.CompileType(
            """
            /// <summary>
            /// Here is some text. <c>it</c> is <see langword="true"/>
            /// that <typeparamref name="T"/> is a type parameter.
            /// </summary>
            class Foo<T>;
            """, "Foo`1", out var compilation);
        var semanticModel = XmlDocsSemanticModel.Create(compilation, TestContext.Current.CancellationToken);

        // Act
        var actual = XmlDocsProvider
            .Create(semanticModel, TestContext.Current.CancellationToken)
            .GetMemberXmlDocs(type);

        // Assert
        var expected = Member(
            Summary(
                Text("Here is some text. "),
                InlineCode("it"),
                Text(" is "),
                SeeLangword("true"),
                Text("that "),
                Typeparamref("T"),
                Text(" is a type parameter.")
            )
        );
        AssertEqual(expected, actual);
    }

    public static TheoryData<String, String, String, MemberElement> TypeDocsData =>
        new TheoryData<String, String, String, MemberElement>(
        [
            (
                """
                ///
                class Foo;
                """,
                "Foo",
                "",
                Member()
            ),
            (
                """
                /// <summary>Represents a generic type.</summary>
                /// <typeparam name="T">
                /// The first parameter.
                /// </typeparam>
                /// <typeparam name="S">The second parameter.</typeparam>
                class Foo<T, S>;
                """,
                "Foo`2",
                "",
                Member(
                    Summary("Represents a generic type."),
                    Typeparam("T", "The first parameter."),
                    Typeparam("S", "The second parameter."))
            ),
            (
                """
                /// <inheritdoc/>
                class Foo : Bar;
                /// <summary>Bar</summary>
                class Bar;
                """,
                "Foo",
                "",
                Member("Bar")
            ),
            (
                """
                /// <inheritdoc cref="Bar"/>
                class Foo;
                /// <summary>Bar</summary>
                class Bar;
                """,
                "Foo",
                "",
                Member("Bar")
            ),
            (
                """
                class Foo : Bar
                {
                   /// <inheritdoc/>
                   public override void Baz(){}
                }
                class Bar
                {
                   /// <summary>Baz</summary>
                   public virtual void Baz(){}
                }
                """,
                "Foo",
                "Baz",
                Member("Baz")
            ),
            (
                """
                class Foo : Bar
                {
                   /// <inheritdoc/>
                   public override void Baz(){}
                }
                class Bar
                {
                   /// <inheritdoc cref="Foobar"/>
                   public virtual void Baz(){}
                }
                /// <summary>Foobar</summary>
                class Foobar;
                """,
                "Foo",
                "Baz",
                Member("Foobar")
            ),
            (
                """
                class Foo : Bar
                {
                    /// <remarks>BazRemarks</remarks>
                    /// <inheritdoc/>
                    public override void Baz(){}
                }
                class Bar
                {
                   /// <inheritdoc cref="Foo.Baz"/>
                   public virtual void Baz(){}
                }
                """,
                "Foo",
                "Baz",
                Member(Remarks("BazRemarks"))
            ),
            (
                """
                /// <summary>Represents a collection of items.</summary>
                /// <typeparam name="T">The type of items in the collection.</typeparam>
                /// <typeparam name="S">The type used for comparison within the collection.</typeparam>
                public class Collection<T, S> { }
                """,
                "Collection`2",
                "",
                Member(
                    Summary("Represents a collection of items."),
                    Typeparam("T", "The type of items in the collection."),
                    Typeparam("S", "The type used for comparison within the collection."))
            ),
            (
                """
                class Foo
                {
                    /// <summary>
                    /// Calculates the sum of <paramref name="a"/> and <paramref name="b"/>.
                    /// </summary>
                    /// <param name="b">The second operand.</param>
                    /// <param name="a">The first operand.</param>
                    /// <returns>The sum of the two integers.</returns>
                    public int Add(int a, int b) { return a + b; }
                }
                """,
                "Foo",
                "Add",
                Member(
                    Param("a", "The first operand."),
                    Summary(
                        Text("Calculates the sum of "),
                        Paramref("a"),
                        Text(" and "),
                        Paramref("b"),
                        Text(".")),
                    Param("b", "The second operand."),
                    Returns("The sum of the two integers."))
            ),
            (
                """
                class Foo
                {
                    /// <summary>Saves the user profile.</summary>
                    /// <param name="userId">The unique identifier for the user.</param>
                    /// <param name="profileData">The data to save for the user's profile.</param>
                    public void SaveProfile(string userId, UserProfile profileData) { }
                }
                """,
                "Foo",
                "SaveProfile",
                Member(
                    Summary("Saves the user profile."),
                    Param("userId", "The unique identifier for the user."),
                    Param("profileData", "The data to save for the user's profile."))
            ),
            (
                """
                /// <summary>Foo</summary>
                /// <example>
                /// <para>To use this class:</para>
                /// <code>
                /// Foo p = new Foo();
                /// </code>
                /// </example>
                class Foo;
                """,
                "Foo",
                "",
                Member(
                    Summary("Foo"),
                    Example(
                        Para("To use this class:"),
                        BlockCode("Foo p = new Foo();")))
            ),
            (
                """
                /// <summary></summary>
                class Foo;
                """,
                "Foo",
                "",
                Member()
            ),
            (
                """
                class BaseClass {
                    /// <summary>Base method implementation.</summary>
                    public virtual void Method() { }
                }

                class DerivedClass : BaseClass {
                    /// <summary>Derived class implementation of the method.</summary>
                    public override void Method() { }
                }
                """,
                "DerivedClass",
                "Method",
                Member("Derived class implementation of the method.")
            ),
            (
                """
                public class Outer {
                    /// <summary>Represents an inner class.</summary>
                    public class Inner { }
                }
                """,
                "Outer+Inner",
                "",
                Member("Represents an inner class.")
            ),
            (
                """
                class Foo
                {
                    /// <summary>Divides two numbers.</summary>
                    /// <param name="a">The dividend.</param>
                    /// <param name="b">The divisor. Cannot be zero.</param>
                    /// <returns>The result of the division.</returns>
                    /// <exception cref="System.DivideByZeroException">Thrown when b is zero.</exception>
                    public double Divide(double a, double b) => throw null;
                }
                """,
                "Foo",
                "Divide",
                Member(
                    Summary("Divides two numbers."),
                    Param("a", "The dividend."),
                    Param("b", "The divisor. Cannot be zero."),
                    Returns("The result of the division."),
                    Exception("T:System.DivideByZeroException", "Thrown when b is zero."))
            ),
            (
                """
                class Foo
                {
                    /// <summary>Converts a list to an array.</summary>
                    /// <typeparam name="T">The type of elements in the list and array.</typeparam>
                    /// <param name="list">The list to convert.</param>
                    /// <returns>An array containing the same elements as the input list.</returns>
                    public T[] ListToArray<T>(List<T> list)
                        => return list.ToArray();
                }
                """,
                "Foo",
                "ListToArray",
                Member(
                    Summary("Converts a list to an array."),
                    Typeparam("T", "The type of elements in the list and array."),
                    Param("list", "The list to convert."),
                    Returns("An array containing the same elements as the input list."))
            )
            // (
            //     """
            //     /// <summary><inheritdoc/></summary>
            //     class Foo;
            //     /// <summary>Bar</summary>
            //     class Bar;
            //     """,
            //     "Foo",
            //     "",
            //     Member("Bar")
            // )
        ]);

    [Theory]
    [MemberData(nameof(TypeDocsData))]
    public void TestParserForTypeDocs(
        String typeSource,
        String typeName,
        String memberName,
        MemberElement expected)
    {
        // Arrange
        var symbol = TestHelpers.CompileSymbol(
            typeSource,
            typeName,
            memberName,
            out var compilation);
        var semanticModel = XmlDocsSemanticModel.Create(compilation, TestContext.Current.CancellationToken);

        // Act
        var parser = XmlDocsProvider.Create(semanticModel, TestContext.Current.CancellationToken);
        var actual = symbol switch
        {
            INamedTypeSymbol type => parser.GetMemberXmlDocs(type),
            IMethodSymbol method => parser.GetMemberXmlDocs(method),
            IEventSymbol @event => parser.GetMemberXmlDocs(@event),
            IFieldSymbol field => parser.GetMemberXmlDocs(field),
            INamespaceSymbol @namespace => parser.GetMemberXmlDocs(@namespace),
            IPropertySymbol property => parser.GetMemberXmlDocs(property),
            _ => throw new NotImplementedException()
        };

        // Assert
        AssertEqual(expected, actual);
    }

    [Fact]
    public void ParserDoesNotTrimSyntheticCommentWhitespace()
    {
        // Arrange
        var type = TestHelpers.CompileType(
            """
            /// <summary>
            /// A sample type.
            /// </summary>
            class Foo;
            """, "Foo", out var compilation);
        var semanticModel = XmlDocsSemanticModel.Create(compilation, TestContext.Current.CancellationToken);

        // Act
        var actual = XmlDocsProvider.Create(
                semanticModel,
                XmlDocsProviderOptions.Default with { TrimmableWhitespace = String.Empty },
                TestContext.Current.CancellationToken)
            .GetMemberXmlDocs(type);

        // Assert
        var expected = Member(Summary("\n    A sample type.\n    "));
        AssertEqual(expected, actual);
    }
}
