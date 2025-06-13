using System.Collections.Immutable;

internal sealed class DigraphNode<TChild>(
    String id,
    ImmutableHashSet<TChild> children,
    ImmutableArray<String> referenceIds)
{
    public String Id { get; } = id;
    public ImmutableHashSet<TChild> Children { get; } = children;
    public ImmutableArray<String> ReferenceIds { get; } = referenceIds;
}
