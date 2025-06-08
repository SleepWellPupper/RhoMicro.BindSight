namespace ReferenceGenerator.XmlDocs;

public class SymbolExtensions
{
    // public static ISymbol GetDeclaringBaseMember(this ISymbol member)
    // {
    //     if (member.ContainingType is not { } containingType)
    //         return member;
    //
    //     var baseMembers =
    //     if (member.IsOverride)
    //     {
    //
    //     }
    //
    //     foreach (var interfaceCandidate in containingType.AllInterfaces)
    //     {
    //         foreach (var memberCandidate in interfaceCandidate.GetMembers())
    //         {
    //             if (memberCandidate.Kind != member.Kind)
    //                 continue;
    //
    //             var implementation = containingType.FindImplementationForInterfaceMember(memberCandidate);
    //
    //             if (SymbolEqualityComparer.Default.Equals(implementation, member))
    //             {
    //                 declaringMember = interfaceCandidate;
    //                 return true;
    //             }
    //         }
    //     }
    //
    //     declaringMember = null;
    //     return false;
    // }
}
