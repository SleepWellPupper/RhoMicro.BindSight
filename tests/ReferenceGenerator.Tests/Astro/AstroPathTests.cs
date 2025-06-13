namespace ReferenceGenerator.Tests;

using Astro;

public class AstroPathTests
{
    sealed class OptionsMock : IAstroReferenceOptions
    {
        public static IAstroReferenceOptions Instance { get; } = new OptionsMock();
        public String ProjectPath { get; set; } = "src/Project.csproj";
        public String ReferenceDirectory { get; set; } = "docs/references/";
        public String RelativeReferencePathBase { get; set; } = "site/references/";
        public String ExternalReferenceUrlFormat { get; set; } = "external.url/{0}";
        public Boolean ClearReferenceDirectory { get; set; }
        public Boolean DryRun { get; set; }
    }

    [Theory]
    [InlineData("class Foo;", "Foo", "site/references/foo/")]
    [InlineData("class Foo<T>;", "Foo`1", "site/references/foo_t/")]
    [InlineData("class Foo<T,S>;", "Foo`2", "site/references/foo_t_s/")]
    [InlineData("namespace Bar{class Foo;}", "Bar.Foo", "site/references/bar/foo/")]
    [InlineData("namespace Bar{class Foo<T>;}", "Bar.Foo`1", "site/references/bar/foo_t/")]
    [InlineData("namespace Bar{class Foo<T,S>;}", "Bar.Foo`2", "site/references/bar/foo_t_s/")]
    [InlineData("namespace Bar.Baz{class Foo;}", "Bar.Baz.Foo", "site/references/barbaz/foo/")]
    [InlineData("namespace Bar.Baz{class Foo<T>;}", "Bar.Baz.Foo`1", "site/references/barbaz/foo_t/")]
    [InlineData("namespace Bar.Baz{class Foo<T,S>;}", "Bar.Baz.Foo`2", "site/references/barbaz/foo_t_s/")]
    public void AnchorHrefParsesTypePathCorrectly(String source, String name, String expected)
    {
        // Arrange
        var type = TestHelpers.CompileType(source, name, out _);

        // Act
        var actual = AnchorHref.CreateForType(type, OptionsMock.Instance).AsString;

        // Assert
        Assert.Equal(expected, actual);
    }

    public static TheoryData<String, String, String, String> GetMemberPathData()
    {
        var data = new MatrixTheoryData<
            (String sourceFormat, String typeNameFormat, String memberNameFormat, String expectedFormat),
            (String sourceFormat, String typeNameFormat, String memberNameFormat, String expectedFormat),
            (String sourceFormat, String typeNameFormat, String memberNameFormat, String expectedFormat)>(
            [
                (
                    "{0}",
                    "{0}",
                    "{0}",
                    $"{OptionsMock.Instance.RelativeReferencePathBase}{{0}}"
                ),
                (
                    "namespace NFoo { {0} }",
                    "NFoo.{0}",
                    "{0}",
                    $"{OptionsMock.Instance.RelativeReferencePathBase}nfoo/{{0}}"
                ),
                (
                    "namespace NFoo.NBar { {0} }",
                    "NFoo.NBar.{0}",
                    "{0}",
                    $"{OptionsMock.Instance.RelativeReferencePathBase}nfoonbar/{{0}}"
                ),
                (
                    "namespace NFoo.NBar.NBaz { {0} }",
                    "NFoo.NBar.NBaz.{0}",
                    "{0}",
                    $"{OptionsMock.Instance.RelativeReferencePathBase}nfoonbarnbaz/{{0}}"
                )
            ],
            [
                (
                    "class TFoo { {0} }",
                    "TFoo{0}",
                    "{0}",
                    $"tfoo/{{0}}"
                ),
                (
                    "class TFoo<T> { {0} }",
                    "TFoo`1{0}",
                    "{0}",
                    $"tfoo_t/{{0}}"
                ),
                (
                    "class TFoo<T,S> { {0} }",
                    "TFoo`2{0}",
                    "{0}",
                    $"tfoo_t_s/{{0}}"
                ),
                (
                    "class TFoo { class TBar { {0} } }",
                    "TFoo+TBar{0}",
                    "{0}",
                    $"tfoo_tbar/{{0}}"
                ),
                (
                    "class TFoo<T> { class TBar { {0} } }",
                    "TFoo`1+TBar{0}",
                    "{0}",
                    $"tfoo_t_tbar/{{0}}"
                ),
                (
                    "class TFoo<T,S> { class TBar { {0} } }",
                    "TFoo`2+TBar{0}",
                    "{0}",
                    $"tfoo_t_s_tbar/{{0}}"
                ),
                (
                    "class TFoo { class TBar<T> { {0} } }",
                    "TFoo+TBar`1{0}",
                    "{0}",
                    $"tfoo_tbar_t/{{0}}"
                ),
                (
                    "class TFoo<T> { class TBar<S> { {0} } }",
                    "TFoo`1+TBar`1{0}",
                    "{0}",
                    $"tfoo_t_tbar_s/{{0}}"
                ),
                (
                    "class TFoo<T,S> { class TBar<U> { {0} } }",
                    "TFoo`2+TBar`1{0}",
                    "{0}",
                    $"tfoo_t_s_tbar_u/{{0}}"
                ),
                (
                    "class TFoo { class TBar<T,S> { {0} } }",
                    "TFoo+TBar`2{0}",
                    "{0}",
                    $"tfoo_tbar_t_s/{{0}}"
                ),
                (
                    "class TFoo<T> { class TBar<S,U> { {0} } }",
                    "TFoo`1+TBar`2{0}",
                    "{0}",
                    $"tfoo_t_tbar_s_u/{{0}}"
                ),
                (
                    "class TFoo<T,S> { class TBar<U,V> { {0} } }",
                    "TFoo`2+TBar`2{0}",
                    "{0}",
                    $"tfoo_t_s_tbar_u_v/{{0}}"
                ),
            ],
            [
                (
                    "void TestMember() { }",
                    "",
                    "TestMember",
                    "#testmember"
                ),
                (
                    "void TestMember<A>() { }",
                    "",
                    "TestMember",
                    "#testmember_a"
                ),
                (
                    "void TestMember<A, B>() { }",
                    "",
                    "TestMember",
                    "#testmember_a_b"
                ),
                (
                    "void TestMember(string a) { }",
                    "",
                    "TestMember",
                    "#testmember_string"
                ),
                (
                    "void TestMember<A>(string a) { }",
                    "",
                    "TestMember",
                    "#testmember_a_string"
                ),
                (
                    "void TestMember<A, B>(string a) { }",
                    "",
                    "TestMember",
                    "#testmember_a_b_string"
                ),
                (
                    "void TestMember(string a, int b) { }",
                    "",
                    "TestMember",
                    "#testmember_string_int"
                ),
                (
                    "void TestMember<A>(string a, int b) { }",
                    "",
                    "TestMember",
                    "#testmember_a_string_int"
                ),
                (
                    "void TestMember<A, B>(string a, int b) { }",
                    "",
                    "TestMember",
                    "#testmember_a_b_string_int"
                ),
                (
                    "void TestMember<A>(A a) { }",
                    "",
                    "TestMember",
                    "#testmember_a_a"
                ),
                (
                    "void TestMember<A, B>(A a, B b) { }",
                    "",
                    "TestMember",
                    "#testmember_a_b_a_b"
                ),
                (
                    "void TestMember(System.Collections.Generic.List<int> a) { }",
                    "",
                    "TestMember",
                    "#testmember_list_int"
                ),
                (
                    "void TestMember(System.Collections.Generic.Dictionary<int, string> a) { }",
                    "",
                    "TestMember",
                    "#testmember_dictionary_int_string"
                ),
                (
                    "void TestMember<A>(System.Collections.Generic.List<int> a, System.Collections.Generic.IEnumerable<A> b) { }",
                    "",
                    "TestMember",
                    "#testmember_a_list_int_ienumerable_a"
                ),
                (
                    "void TestMember<A>(System.Collections.Generic.Dictionary<int, string> a, System.Collections.Generic.IEnumerable<A> b) { }",
                    "",
                    "TestMember",
                    "#testmember_a_dictionary_int_string_ienumerable_a"
                ),
                (
                    "int TestMember { get; }",
                    "",
                    "TestMember",
                    "#testmember"
                ),
                (
                    "int TestMember { get; set; }",
                    "",
                    "TestMember",
                    "#testmember"
                ),
                (
                    "int TestMember;",
                    "",
                    "TestMember",
                    "#testmember"
                ),
                (
                    "event Action<int> TestMember;",
                    "",
                    "TestMember",
                    "#testmember"
                ),
                //TODO operators, constructors, finalizers
            ]);
        var result = new TheoryData<String, String, String, String>();
        foreach (var (first, second, third) in data.Select(r => r.Data))
        {
            var source = Combine(first.sourceFormat, second.sourceFormat, third.sourceFormat);
            var typeName = Combine(first.typeNameFormat, second.typeNameFormat, third.typeNameFormat);
            var memberName = Combine(first.memberNameFormat, second.memberNameFormat, third.memberNameFormat);
            var expected = Combine(first.expectedFormat, second.expectedFormat, third.expectedFormat);

            result.Add(source, typeName, memberName, expected);
        }

        return result;
    }

    private static String Combine(String first, String second, String third) =>
        first.Replace("{0}", second).Replace("{0}", third);

    [Theory]
    [MemberData(nameof(GetMemberPathData))]
    public void AnchorHrefParsesMemberPathCorrectly(String source, String typeName, String memberName, String expected)
    {
        // Arrange
        var type = TestHelpers.CompileSymbol(source, typeName, memberName, out _);

        // Act
        var actual = AnchorHref.Create(type, OptionsMock.Instance).AsString;

        // Assert
        Assert.Equal(expected, actual);
    }
}
