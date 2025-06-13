namespace ReferenceGenerator.Astro;

using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Library.Text.Templating;

public record struct AnchorHref
{
    private AnchorHref(String value, Boolean isExternal)
    {
        Value = value;
        IsExternal = isExternal;
    }

    public String Value { get; }
    public Boolean IsExternal { get; }

    public static AnchorHref Empty => new(String.Empty, isExternal: false);

    public static AnchorHref CreateExternal(ISymbol symbol, IAstroReferenceOptions options)
    {
        var normalizedSymbol = symbol switch
        {
            INamedTypeSymbol t => t.ConstructedFrom,
            IMethodSymbol m => m.ConstructedFrom,
            _ => symbol
        };

        var value = String.Format(
            options.ExternalReferenceUrlFormat,
            Uri.EscapeDataString(
                normalizedSymbol.ToDisplayString(SymbolDisplayFormats.ExternalReferenceFormat)));

        return new AnchorHref(value, isExternal: true);
    }

    public static AnchorHref Create(ISymbol symbol, IAstroReferenceOptions options)
    {
        return symbol switch
        {
            INamedTypeSymbol type => CreateForType(type, options),
            IMethodSymbol method => CreateForMethod(method, options),
            IEventSymbol @event => CreateForEvent(@event, options),
            IPropertySymbol property => CreateForProperty(property, options),
            IFieldSymbol field => CreateForField(field, options),
            _ => Empty
        };
    }

    public static AnchorHref CreateForField(IFieldSymbol field, IAstroReferenceOptions options)
    {
        var buffer = new DynamicallyAllocatedCharBuffer(stackalloc Char[256]);
        try
        {
            AppendField(field, ref buffer, options);
        }
        finally
        {
            buffer.Dispose();
        }

        var value = buffer.ToString();

        return new AnchorHref(value, isExternal: false);
    }

    public static AnchorHref CreateForEvent(IEventSymbol @event, IAstroReferenceOptions options)
    {
        var buffer = new DynamicallyAllocatedCharBuffer(stackalloc Char[256]);
        try
        {
            AppendEvent(@event, ref buffer, options);
        }
        finally
        {
            buffer.Dispose();
        }

        var value = buffer.ToString();

        return new AnchorHref(value, isExternal: false);
    }

    public static AnchorHref CreateForProperty(IPropertySymbol property, IAstroReferenceOptions options)
    {
        var buffer = new DynamicallyAllocatedCharBuffer(stackalloc Char[256]);
        try
        {
            AppendProperty(property, ref buffer, options);
        }
        finally
        {
            buffer.Dispose();
        }

        var value = buffer.ToString();

        return new AnchorHref(value, isExternal: false);
    }

    public static AnchorHref CreateForMethod(IMethodSymbol method, IAstroReferenceOptions options)
    {
        var buffer = new DynamicallyAllocatedCharBuffer(stackalloc Char[256]);
        try
        {
            AppendMethod(method, ref buffer, options);
        }
        finally
        {
            buffer.Dispose();
        }

        var value = buffer.ToString();

        return new AnchorHref(value, isExternal: false);
    }

    private static void AppendMethod(
        IMethodSymbol method,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        AppendType(method.ContainingType, ref buffer, options);
        buffer.Add('#');
        AppendName(method, ref buffer);
        AppendParameters(method, ref buffer);
    }

    private static void AppendEvent(
        IEventSymbol @event,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        AppendType(@event.ContainingType, ref buffer, options);
        buffer.Add('#');
        AppendName(@event, ref buffer);
    }

    private static void AppendField(
        IFieldSymbol field,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        AppendType(field.ContainingType, ref buffer, options);
        buffer.Add('#');
        AppendName(field, ref buffer);
    }

    private static void AppendProperty(
        IPropertySymbol property,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        AppendType(property.ContainingType, ref buffer, options);
        buffer.Add('#');
        AppendName(property, ref buffer);
    }

    private static void AppendParameters(IMethodSymbol method, ref DynamicallyAllocatedCharBuffer buffer)
    {
        foreach (var parameter in method.Parameters)
        {
            buffer.Add('_');
            AppendType(parameter.Type, ref buffer);
        }
    }

    public static AnchorHref CreateForType(INamedTypeSymbol type, IAstroReferenceOptions options)
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

        return new AnchorHref(value, isExternal: false);
    }

    private static void AppendType(
        INamedTypeSymbol type,
        ref DynamicallyAllocatedCharBuffer buffer,
        IAstroReferenceOptions options)
    {
        buffer.Add(options.RelativeReferencePathBase);
        if (buffer.Span is not [.., '/'])
            buffer.Add('/');

        if (type.ContainingNamespace is { IsGlobalNamespace: false } @namespace)
        {
            AppendNamespace(@namespace, ref buffer);
            buffer.Add('/');
        }

        AppendType(type, ref buffer);
        buffer.Add('/');
    }

    private static void AppendType(ITypeSymbol type, ref DynamicallyAllocatedCharBuffer buffer)
    {
        if (type is { Kind: not SymbolKind.TypeParameter, ContainingType : { } containingType })
        {
            AppendType(containingType, ref buffer);
            buffer.Add('_');
        }

        AppendName(type, ref buffer);
    }

    private static void AppendName(ISymbol symbol, ref DynamicallyAllocatedCharBuffer buffer)
    {
        var name = symbol.ToDisplayString(SymbolDisplayFormats.NameOrSpecialName);

        name.AsSpan().ToLowerInvariant(buffer.Reserve(name.Length));

        var typeArguments = symbol switch
        {
            INamedTypeSymbol type => type.TypeArguments,
            IMethodSymbol method => method.TypeArguments,
            _ => []
        };

        foreach (var typeArgument in typeArguments)
        {
            buffer.Add('_');
            AppendName(typeArgument, ref buffer);
        }
    }

    private static void AppendNamespace(INamespaceSymbol @namespace, ref DynamicallyAllocatedCharBuffer buffer)
    {
        if (@namespace.ContainingNamespace is { IsGlobalNamespace: false } containingNamespace)
            AppendNamespace(containingNamespace, ref buffer);

        @namespace.Name.AsSpan().ToLowerInvariant(buffer.Reserve(@namespace.Name.Length));
    }
}
