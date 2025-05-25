namespace RhoMicro.OptionsDocs.Transformations;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using Enrichments;
using Models;

/// <summary>
/// Replaces option model keys.
/// </summary>
/// <param name="replacementsProvider">
/// The provider providing key replacements.
/// </param>
public sealed class KeyReplacementTransformation(IKeyReplacementsProvider replacementsProvider)
    : IOptionsModelTransformation
{
    private readonly KeyReplacementTrie _trie =
        KeyReplacementTrie.Create(replacementsProvider.GetReplacements());

    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newChildren = ImmutableHashSet.CreateBuilder<OptionsModelChild>();

        foreach (var child in model.Children)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var newChild = child;

            if (_trie.TryGetReplaced(child.GetKey(), out String? newChildKey))
                newChild = child.WithEnrichment(new ChildKeyEnrichment(child, newChildKey, combineWithParent: false));

            newChildren.Add(newChild);
        }

        var result = model.WithChildren(newChildren.ToImmutable());

        if (_trie.TryGetReplaced(model.Root.GetKey(), out String? newRootKey))
        {
            var newRoot = model.Root.WithEnrichment(new RootKeyEnrichment(newRootKey));
            result = result.WithRoot(newRoot);
        }

        return ValueTask.FromResult<OptionsModel?>(result);
    }
}
