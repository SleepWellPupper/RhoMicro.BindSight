namespace RhoMicro.BindSight.Transformations;

using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Implements a key replacement algorithm.
/// </summary>
/// <remarks>
/// <code>
/// Construction
/// We construct a trie that associates replacement prefixes
/// with their replacement value.
/// Keys are split into tokens via the ':' separator, which
/// is defined as the conventional separator for configuration
/// keys.
/// The parent of a node with an associated replacement rule
/// is the ordered list of tokens in the key up to that token.
///
/// Search
/// We traverse the trie in order of tokens in a given key
/// until we hit a node that does not allow further traversal,
/// i.e. its children do not contain the next token.
/// While traversing the trie, we memorize the current nodes
/// replacement rule if it is associated with one, as well as
/// the token index up to that point.
/// Upon reaching the last navigable node, we then apply the
/// memorized replacement up to the memorized index.
/// This way, the maximum depth traversed will be linearly
/// proportional to the amount of tokens in a given key.
///
/// The time complexity for a single lookup is therefore O(n).
///
/// a:b
/// a:b
/// a:b:c
/// a:b:d
/// b:a:b:b:b
/// a:b:c:d:e
/// b:d:e
///
/// a set of rules like such:
/// a:b -> f:g:h
/// a:b:c -> x:y:z
/// a:d -> m
/// a:b:d:e:f -> v
/// u -> w
/// is built into a trie like this:
/// _
/// _:a
/// _:a:b -> f:g:h
/// _:a:b:c -> x:y:z
/// _:a:d -> m
/// _:u -> w
///
/// a:b -> f:g:h
/// a:b:c -> f:g:h:c
/// a:b:d -> f:g:h:d
/// a:b:c:d:e -> f:g:h:c:d:e
/// b:d:e -> b:d:e
/// </code>
/// </remarks>
public sealed class KeyReplacementTrie
{
    private sealed class Node
    {
        private Node(StringSegment value, Node? parent, String? replacementValue)
        {
            Value = value;
            Parent = parent;
            ReplacementValue = replacementValue;
        }

        public String? ReplacementValue { get; }
        public StringSegment Value { get; }
        public Node? Parent { get; }

        [MemberNotNullWhen(false, nameof(Parent))]
        public Boolean IsRoot => Parent is null;

        private readonly Dictionary<StringSegment, Node> _children = [];

        public StringSegment Key => IsRoot
            ? StringSegment.Empty
            : $"{(Parent.IsRoot ? String.Empty : $"{Parent.Key}:")}{Value}";

        public Boolean TryGetReplacementRule(
            String key,
            [NotNullWhen(true)] out String? replacementValue,
            out Int32 replacementLength)
        {
            replacementLength = 0;
            replacementValue = null;

            // We may only start traversal from the root.
            if (!IsRoot)
                return false;

            var rentedRanges = ArrayPool<Range>.Shared.Rent(key.Length);
            try
            {
                var ranges = rentedRanges.AsSpan(0, key.Length);
                Int32 tokensCount = key.AsSpan().Split(
                    ranges,
                    ':',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                ranges = ranges[..tokensCount];

                // The key did not contain any sections.
                if (tokensCount is 0)
                    return false;

                var currentNode = this;

                foreach (var range in ranges)
                {
                    (Int32 offset, Int32 length) = range.GetOffsetAndLength(key.Length);
                    var value = new StringSegment(buffer: key, offset: offset, length: length);

                    // Did we reach the last matching prefix path?
                    if (!currentNode._children.TryGetValue(value, out var matchingChild))
                        break;

                    // We memorize the last matched replacement value.
                    if (matchingChild.ReplacementValue is { } r)
                    {
                        replacementValue = r;
                        replacementLength = value.Offset + value.Length;
                    }

                    currentNode = matchingChild;
                }

                // Did we match a prefix path?
                if (replacementValue is not null)
                    return true;

                return false;
            }
            finally
            {
                ArrayPool<Range>.Shared.Return(rentedRanges);
            }
        }

        private void Combine(Node node)
        {
            foreach (var (_, theirChild) in node._children)
            {
                if (_children.TryGetValue(theirChild.Value, out var ourOverlappingChild))
                {
                    // Replacement values can only be present on leaves, so a collision
                    // indicates a duplicate key in the input map, which is only possible
                    // by having empty sections like 'a::b'.
                    if (ourOverlappingChild.ReplacementValue is { } first && theirChild.ReplacementValue is { } second)
                    {
                        throw new InvalidOperationException(
                            "Encountered replacement value collision for rules " +
                            $"'{ourOverlappingChild.Value.Buffer}'->'{ourOverlappingChild.ReplacementValue}' and " +
                            $"'{theirChild.Value.Buffer}'->{theirChild.ReplacementValue} " +
                            "It is possible to accidentally define overlapping keys by using invalid values. For " +
                            "example, 'a::b'->'c' would collide with 'a:b'->'d', as empty key sections are ignored.");
                    }

                    ourOverlappingChild.Combine(theirChild);
                }
                else
                {
                    _children.Add(theirChild.Value, theirChild);
                }
            }
        }

        public static Node CreateRoot(IReadOnlyDictionary<String, String> replacementValues)
        {
            var result = new Node(value: String.Empty, parent: null, replacementValue: null);
            foreach ((String key, String value) in replacementValues)
                AddChild(key, parent: result, leafReplacementValue: value);

            return result;
        }

        private static void AddChild(String key, Node parent, String leafReplacementValue)
        {
            var rentedRanges = ArrayPool<Range>.Shared.Rent(key.Length);
            try
            {
                var ranges = rentedRanges.AsSpan(0, key.Length);
                Int32 tokensCount = key.AsSpan().Split(
                    ranges,
                    ':',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                ranges = ranges[..tokensCount];

                if (tokensCount is 0)
                    return;

                Node? firstChild = null;
                var currentParent = parent;

                for (Int32 i = 0; i < ranges.Length; i++)
                {
                    Range range = ranges[i];
                    (Int32 offset, Int32 length) = range.GetOffsetAndLength(key.Length);
                    var value = new StringSegment(buffer: key, offset: offset, length: length);

                    String? replacementValue = i == ranges.Length - 1 ? leafReplacementValue : null;

                    var newChild = new Node(value, currentParent, replacementValue);
                    // We only add to the new parent (created in the previous iteration)
                    // and add to the target parent using .Combine later.
                    if (currentParent != parent)
                        currentParent._children.Add(value, newChild);
                    firstChild ??= newChild;
                    currentParent = newChild;
                }

                Debug.Assert(firstChild is not null);

                // .Combine recursively merges shared branch prefixes.
                if (parent._children.TryGetValue(firstChild.Value, out var overlappingChild))
                    overlappingChild.Combine(firstChild);
                else
                    parent._children.Add(firstChild.Value, firstChild);

                return;
            }
            finally
            {
                ArrayPool<Range>.Shared.Return(rentedRanges);
            }
        }

        public override String ToString() =>
            IsRoot
                ? String.Join(", ", _children)
                : ReplacementValue is not null
                    ? $"'{Key}' -> '{ReplacementValue}'"
                    : $"'{Key}'";
    }

    private readonly Node _root;

    private KeyReplacementTrie(Node root)
    {
        _root = root;
    }

    /// <summary>
    /// Creates a new trie from a set of keys and associated replacement.
    /// </summary>
    /// <param name="replacements">
    /// The dictionary providing keys and their replacement,
    /// </param>
    /// <returns>
    /// A new replacement trie.
    /// </returns>
    public static KeyReplacementTrie Create(IReadOnlyDictionary<String, String> replacements)
    {
        var root = Node.CreateRoot(replacements);
        var result = new KeyReplacementTrie(root);

        return result;
    }

    /// <summary>
    /// Attempts to locate a replacement key prefixing <paramref name="key"/> and
    /// applies it to <paramref name="key"/>.
    /// </summary>
    /// <param name="key">
    /// The key to locate a replacement for.
    /// </param>
    /// <param name="replaced">
    /// The replaced key if a replacement could be located; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a replacement could be located; otherwise, <see langword="false"/>.
    /// </returns>
    public Boolean TryGetReplaced(String key, [NotNullWhen(true)] out String? replaced)
    {
        if (!_root.TryGetReplacementRule(key, out String? replacementValue, out Int32 replacementLength))
        {
            replaced = null;
            return false;
        }

        // Complete replacement?
        if (replacementLength == key.Length)
            replaced = replacementValue;
        // Complete removal?
        else if (replacementValue.Length is 0)
            replaced = key[replacementLength..];
        // Partial replacement
        else
            replaced = $"{replacementValue}{key.AsSpan(replacementLength)}";

        return true;
    }
}
