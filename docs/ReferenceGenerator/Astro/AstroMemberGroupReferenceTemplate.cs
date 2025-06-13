namespace ReferenceGenerator.Astro;

using System.Collections.Immutable;
using RhoMicro.CodeAnalysis;

[Template(
    """
    {:if(members is []) return;:}
    ### (:name:)

    {:
        foreach(var member in members)
        {
            (:new AstroMemberReferenceTemplate(member, referencePaths):)
        }
    :}

    """
)]
internal readonly partial struct AstroMemberGroupReferenceTemplate(
    ImmutableArray<MemberDocumentationModel> members,
    String name,
    AstroReferencePathsContext referencePaths);
