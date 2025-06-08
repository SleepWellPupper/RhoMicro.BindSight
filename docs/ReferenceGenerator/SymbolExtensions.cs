namespace ReferenceGenerator;

using Microsoft.CodeAnalysis;

internal static class SymbolExtensions
{
    public static String GetMemberName(this ISymbol symbol) =>
        symbol is IMethodSymbol
        {
            MethodKind: MethodKind.Constructor,
            CanBeReferencedByName: false
        }
            ? symbol.ContainingType.Name
            : symbol.Name;

}
