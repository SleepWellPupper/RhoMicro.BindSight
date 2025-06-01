using System.Reflection;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RhoMicro.BindSight.Tests;

using Models;

public class OptionsTypeBindingRuleTests
{
    public static TheoryData<String, Type, PropertyInfo[]> GetDefaultBindableData()
    {
        var ct = TestContext.Current.CancellationToken;

        var bindablePropertyAttribute = """
                                        internal class TestBindablePropertyAttribute:System.Attribute;
                                        """;
        var optionsAttribute = """
                               internal class TestOptionsAttribute(string description):System.Attribute
                               {
                                   public string Description => description;
                               }
                               """;
        var modifiers = new[] { "public", "internal", "private" };
        var setters = new[] { "set; ", "" };
        var listLikeTypes = s_mutableListTypes.Concat(s_immutableListTypes).ToList();
        var mapLikeTypes = s_mutableDictionaryTypes.Concat(s_immutableDictionaryTypes).ToList();
        var combinations = modifiers.SelectMany(modifier => setters.Select(setter => (modifier, setter))).ToList();
        var sources = new List<String>();

        foreach (var (modifier, setter, type) in
                 listLikeTypes.SelectMany(type =>
                     combinations.Select(c => (c.modifier, c.setter, type))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("unbindable {{modifier}}{{(setter is "" ? " " : " set ")}}GenericArgumentType {{(s_mutableListTypes.Contains(type) ? "mutable" : "immutable")}} item")]
                  class TestType_{{sources.Count}}
                  {
                      {{modifier}} {{type.FullName![..^2]}}<System.Type> Property { get; {{setter}}}
                  }
                  """);
        }

        foreach (var (modifier, setter, type) in
                 mapLikeTypes.SelectMany(type =>
                     combinations.Select(c => (c.modifier, c.setter, type))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("unbindable {{modifier}}{{(setter is "" ? " " : " set ")}}GenericArgumentType {{(s_mutableDictionaryTypes.Contains(type) ? "mutable" : "immutable")}} value")]
                  class TestType_{{sources.Count}}
                  {
                      {{modifier}} {{type.FullName![..^2]}}<string, System.Type> Property { get; {{setter}}}
                  }
                  """);
        }

        foreach (var (modifier, setter, type, primitive) in
                 mapLikeTypes.SelectMany(type =>
                     combinations.SelectMany(c =>
                         s_primitives.Except([typeof(String)])
                             .Select(primitive => (c.modifier, c.setter, type, primitive)))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("unbindable {{modifier}}{{(modifier is "public" ? " " : " set ")}}primitive {{(s_mutableDictionaryTypes.Contains(type) ? "mutable" : "immutable")}} key")]
                  class TestType_{{sources.Count}}
                  {
                      {{modifier}} {{type.FullName![..^2]}}<{{primitive.FullName}}, string> Property { get; {{setter}}}
                  }
                  """);
        }

        foreach (var (modifier, primitive) in
                 s_primitives.SelectMany(primitive =>
                     modifiers.Select(modifier => (modifier, primitive))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("unbindable {{modifier}}{{(modifier is "public" ? " " : " set ")}}primitive property")]
                  class TestType_{{sources.Count}}
                  {
                      {{modifier}} {{primitive.FullName}} Property { get; {{(modifier is "public" ? "" : "set; ")}}}
                  }
                  """);
        }

        foreach (var primitive in s_primitives)
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("bindable public set primitive property")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{primitive.FullName}} Property { get; set; }
                  }
                  """);
        }

        foreach (var (type, primitive, setter) in
                 s_mutableListTypes.SelectMany(type =>
                     s_primitives.SelectMany(primitive =>
                         setters.Select(setter => (type, primitive, setter)))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("bindable public{{(setter is [] ? " " : " set ")}}primitive mutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<{{primitive}}> Property { get; {{setter}}}
                  }
                  """);
        }

        foreach (var (type, elementType, setter) in s_mutableListTypes.SelectMany(type =>
                     listLikeTypes.SelectMany(t =>
                             s_primitives.Select(p => t.MakeGenericType(p)))
                         .SelectMany(elementType =>
                             setters.Select(setter => (type, primitive: elementType, setter)))))
        {
            sources.Add(
                $$"""
                  [TestOptionsAttribute("bindable public{{(setter is [] ? " " : " set ")}}primitive nested mutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<{{elementType.GetGenericTypeDefinition().FullName![..^2]}}<{{elementType.GenericTypeArguments[0].FullName}}>> Property { get; {{setter}}}
                  }
                  """);
        }

        foreach (var (type, elementType) in
                 s_mutableListTypes.SelectMany(type =>
                     listLikeTypes.Select(elementType => (type, elementType))))
        {
            sources.Add(
                $$"""
                  public class Foo_{{sources.Count}};

                  [TestOptionsAttribute("bindable public custom nested mutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<{{elementType.GetGenericTypeDefinition().FullName![..^2]}}<Foo_{{sources.Count}}>> Property { get; }
                  }
                  """);
        }

        foreach (var (type, elementType) in
                 s_immutableListTypes.SelectMany(type =>
                     listLikeTypes.Select(elementType => (type, elementType))))
        {
            sources.Add(
                $$"""
                  public class Foo_{{sources.Count}};

                  [TestOptionsAttribute("bindable public set custom nested immutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<{{elementType.GetGenericTypeDefinition().FullName![..^2]}}<Foo_{{sources.Count}}>> Property { get; set; }
                  }
                  """);
        }

        foreach (var type in s_immutableListTypes)
        {
            sources.Add(
                $$"""
                  public class Foo_{{sources.Count}};

                  [TestOptionsAttribute("bindable public set custom immutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<Foo_{{sources.Count}}> Property { get; set; }
                  }
                  """);
        }

        foreach (var type in s_mutableListTypes)
        {
            sources.Add(
                $$"""
                  public class Foo_{{sources.Count}};

                  [TestOptionsAttribute("bindable public custom mutable item")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public {{type.FullName![..^2]}}<Foo_{{sources.Count}}> Property { get; }
                  }
                  """);
        }

        {
            sources.Add(
                $$"""
                  public class Foo_{{sources.Count}};

                  [TestOptionsAttribute("bindable public set custom property")]
                  class TestType_{{sources.Count}}
                  {
                      [TestBindablePropertyAttribute]
                      public Foo_{{sources.Count}} Property { get; set; }
                  }
                  """);
        }

        var result = new TheoryData<String, Type, PropertyInfo[]>();

        var parseOptions = new CSharpParseOptions(
            LanguageVersion.CSharp13,
            DocumentationMode.None);

        var trees = sources
            .Concat([optionsAttribute, bindablePropertyAttribute])
            .Select(s => CSharpSyntaxTree.ParseText(s, parseOptions, cancellationToken: ct));

        using var ms = new MemoryStream();
        var emitResult = CSharpCompilation.Create(
                "test_assembly",
                trees,
                Net80.References.All,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .Emit(ms, cancellationToken: ct);

        Assert.True(emitResult.Success);

        var testAssembly = Assembly.Load(ms.ToArray());

        var testTypes = testAssembly
            .GetTypes()
            .Where(t => t.GetCustomAttributes().Any(a => a.GetType().FullName is "TestOptionsAttribute"));

        foreach (var testType in testTypes)
        {
            var expectedProperties = testType
                .GetProperties()
                .Where(p => p.GetCustomAttributes().Any(a => a.GetType().FullName is "TestBindablePropertyAttribute"))
                .ToArray();

            var attribute = testType.GetCustomAttributes().Single(a => a.GetType().Name is "TestOptionsAttribute");
            var description = (String)attribute.GetType().GetProperty("Description")!.GetMethod!.Invoke(attribute, [])!;

            result.Add(description, testType, expectedProperties);
        }

        return result;
    }

    private static readonly HashSet<Type> s_primitives =
    [
        typeof(Boolean), typeof(Byte), typeof(Char), typeof(Decimal), typeof(Double), typeof(Single),
        typeof(Int32), typeof(Int64), typeof(SByte), typeof(Int16), typeof(UInt32),
        typeof(UInt64), typeof(UInt16), typeof(String),
    ];

    private static readonly HashSet<Type> s_mutableListTypes =
    [
        typeof(List<>), typeof(HashSet<>), typeof(ICollection<>), typeof(ISet<>), typeof(IList<>)
    ];

    private static readonly HashSet<Type> s_immutableListTypes =
    [
        typeof(IEnumerable<>), typeof(IReadOnlyCollection<>), typeof(IReadOnlySet<>), typeof(IReadOnlyList<>)
    ];

    private static readonly HashSet<Type> s_mutableDictionaryTypes =
    [
        typeof(Dictionary<,>), typeof(IDictionary<,>)
    ];

    private static readonly HashSet<Type> s_immutableDictionaryTypes =
    [
        typeof(IReadOnlyDictionary<,>)
    ];

    [Theory]
    [MemberData(nameof(GetDefaultBindableData))]
    public void CreateDefaultReturnsBindableDefaultProperties(String description, Type type, PropertyInfo[] properties)
    {
        // Arrange
        var expectedProperties = properties.ToHashSet();

        // Act
        var actual = OptionsTypeBindingRule.CreateDefault(type);

        // Assert
        Assert.Equal(type, actual.Type);
        Assert.Equal(expectedProperties, actual.BoundPropertiesAccessor.Invoke(type));
    }
}
