using System.Collections.Immutable;

namespace ReferenceGenerator.XmlDocs;

internal sealed class DigraphNode<TChild>(
    String id,
    ImmutableHashSet<TChild> children,
    ImmutableHashSet<String> referenceIds)
{
    public String Id { get; } = id;
    public ImmutableHashSet<TChild> Children { get; } = children;
    public ImmutableHashSet<String> ReferenceIds { get; } = referenceIds;
}
