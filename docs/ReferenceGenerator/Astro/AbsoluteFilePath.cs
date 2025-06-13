namespace ReferenceGenerator.Astro;

using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Library.Text.Templating;

[UnionType<String>(Options = UnionTypeOptions.None)]
public readonly partial struct AbsoluteFilePath
{
    public static AbsoluteFilePath Empty => new(String.Empty);

    private static AbsoluteFilePath Create([UnionTypeFactory] String value) => new(value);

    public static AbsoluteFilePath Create(INamedTypeSymbol type, IAstroReferenceOptions options)
    {
        var buffer = new DynamicallyAllocatedCharBuffer(stackalloc Char[256]);
        try
        {
            AppendType(type, ref buffer, options);
        }
        finally
        {
            buffer.Dispose();
        }

        var value = buffer.ToString();

        return new AbsoluteFilePath(value);
    }

    private static void AppendType(
        INamedTypeSymbol type,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        buffer.Add(options.ReferenceDirectory);
        if (buffer.Span is not [.., '/'])
            buffer.Add('/');

        if (type.ContainingNamespace is { IsGlobalNamespace: false } @namespace)
        {
            AppendNamespace(@namespace, ref buffer);
            buffer.Add('/');
        }

        AppendName(type, ref buffer);
        buffer.Add(".mdx");
    }

    private static void AppendName(INamedTypeSymbol symbol, ref DynamicallyAllocatedCharBuffer buffer)
    {
        buffer.Add(symbol.Name);

        foreach (var typeParameter in symbol.TypeParameters)
        {
            buffer.Add('_');
            buffer.Add(typeParameter.Name);
        }
    }

    private static void AppendNamespace(INamespaceSymbol @namespace, ref DynamicallyAllocatedCharBuffer buffer)
    {
        if (@namespace.ContainingNamespace is { IsGlobalNamespace: false } containingNamespace)
        {
            AppendNamespace(containingNamespace, ref buffer);
            buffer.Add('.');
        }

        buffer.Add(@namespace.Name);
    }
}
