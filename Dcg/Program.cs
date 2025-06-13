// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata;

var graph = new DigraphBuilder<DocsElement>()
    .Add("class1",
        [],
        ["class0"])
    .Add("class2",
        [DocsElement.Summary("Class 2 Summary"), DocsElement.Remarks("Class 2 Remarks")],
        ["class3"])
    .Add("class3",
        [DocsElement.Summary("Class 3 Summary"), DocsElement.Example("Class 3 Example")],
        ["class1"])
    .Build();

Console.WriteLine(graph);

/// <inheritdoc />
class Foo1 : Foo2;

class Foo2 : Foo3;

/// <inheritdoc cref="Object"/>
class Foo3;

readonly struct DocsElement : IEquatable<DocsElement>
{
    private DocsElement(int kind, string text)
    {
        Kind = kind;
        Text = text;
    }

    public static DocsElement Summary(string text) => new(1, text);
    public static DocsElement Remarks(string text) => new(2, text);
    public static DocsElement Example(string text) => new(3, text);

    public bool Equals(DocsElement other) => Kind == other.Kind;

    public override bool Equals(object? obj) => obj is DocsElement other && Equals(other);

    public override int GetHashCode() => Kind;

    public Int32 Kind { get; }
    public String Text { get; }

    public override String ToString() => Text;
}
