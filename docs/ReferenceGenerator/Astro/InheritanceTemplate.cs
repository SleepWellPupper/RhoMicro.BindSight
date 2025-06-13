namespace ReferenceGenerator.Astro;

using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;

[Template(
    """
    (:type.Name:){:
        if(type.BaseType is {} baseType)
        {
            (:" → ":)
            (:new InheritanceTemplate(baseType):)
        }
    :}
    """
)]
internal readonly partial struct InheritanceTemplate(INamedTypeSymbol type);
