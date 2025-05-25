namespace RhoMicro.OptionsDocs.Tests;

using Transformations;

public class KeyReplacementTrieTests
{
    public static TheoryData<IReadOnlyDictionary<string, string>, string, string> GetTestData()
    {
        var result = new TheoryData<IReadOnlyDictionary<string, string>, string, string>();

        var fooBarReplacements = new Dictionary<String, String>() { ["Foo:Bar:Baz"] = "foobar" };

        result.Add(fooBarReplacements, "Foo", "Foo");
        result.Add(fooBarReplacements, "Foo:Bar", "Foo:Bar");
        result.Add(fooBarReplacements, "Foo:Bar:Baz", "foobar");
        result.Add(fooBarReplacements, "Foo:Bar:Baz:Zip", "foobar:Zip");
        result.Add(fooBarReplacements, "Nope:Foo:Bar:Baz", "Nope:Foo:Bar:Baz");
        result.Add(fooBarReplacements, "Nope:Foo:Bar:Baz:Zip", "Nope:Foo:Bar:Baz:Zip");
        result.Add(fooBarReplacements, "Foo:Bar:Baz:Zip:Zap", "foobar:Zip:Zap");

        var testReplacements = new Dictionary<String, string>() { ["TestOptions"] = "Test" };

        result.Add(testReplacements, "TestOptions", "Test");
        result.Add(testReplacements, "TestOptions:StringList", "Test:StringList");
        result.Add(testReplacements, "TestOptions:StringList:[n]", "Test:StringList:[n]");
        result.Add(testReplacements, "TestOptions:NestedSet", "Test:NestedSet");
        result.Add(testReplacements, "TestOptions:NestedSet:[n]", "Test:NestedSet:[n]");
        result.Add(testReplacements, "TestOptions:NestedSet:[n]:Int32Property", "Test:NestedSet:[n]:Int32Property");
        result.Add(testReplacements, "TestOptions:NestedProperty", "Test:NestedProperty");
        result.Add(testReplacements, "TestOptions:NestedProperty:Int32Property", "Test:NestedProperty:Int32Property");

        return result;
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void ProducesExpectedReplacedKey(
        IReadOnlyDictionary<String, String> replacements,
        String key,
        String expected)
    {
        // Arrange
        var trie = KeyReplacementTrie.Create(replacements);

        // Act
        var replaceResult = trie.TryGetReplaced(key, out var actual);

        // Assert
        if (expected != key)
        {
            Assert.True(replaceResult);
            Assert.Equal(expected, actual);
        }
        else
        {
            Assert.False(replaceResult);
            Assert.Null(actual);
        }
    }
}
