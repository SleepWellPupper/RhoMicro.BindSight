namespace RhoMicro.BindSight.Transformations;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using Generators;
using RhoMicro.CodeAnalysis;
using Services;

/// <summary>
/// Provides key replacements to <see cref="KeyReplacementTransformation"/>.
/// </summary>
public sealed class KeyReplacementsBuilder(BindSightBuilder optionsDocs) : IKeyReplacementsProvider
{
    /// <summary>
    /// Gets the wrapped options docs builder.
    /// </summary>
    public BindSightBuilder BindSight => optionsDocs;

    private readonly ConcurrentDictionary<String, String> _replacements = [];

    /// <summary>
    /// Replaces keys prefixed with <paramref name="key"/> with their
    /// prefix replaced by <paramref name="replacement"/>.
    /// </summary>
    /// <param name="key">
    /// The key prefix to replace.
    /// </param>
    /// <param name="replacement">
    /// The replacement prefix.
    /// </param>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public KeyReplacementsBuilder Use(String key, String replacement)
    {
        _replacements[key] = replacement;

        return this;
    }

    /// <summary>
    /// Replaces keys prefixed with <c>typeof(TOptions).Name</c>
    /// with their prefix replaced by <paramref name="replacement"/>.
    /// </summary>
    /// <param name="replacement">
    /// The replacement prefix.
    /// </param>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public KeyReplacementsBuilder Use<TOptions>(String replacement)
        => Use(typeof(TOptions).Name, replacement);

    /// <summary>
    /// Clears out all registered replacements.
    /// </summary>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public KeyReplacementsBuilder Clear()
    {
        _replacements.Clear();

        return this;
    }

    private const Int32 MutableLength = 7;

    private static readonly ImmutableArray<String> _mutableKeys =
    [
        nameof(MutableChildExclusionOptions),
        nameof(MutableReadmeGeneratorOptions),
        nameof(MutableRootExclusionOptions),
        nameof(MutableJsonSchemaGeneratorOptions),
        nameof(MutableBindSightGeneratorOptions),
        nameof(MutableBindSightRunnerOptions),
    ];

    private static readonly ImmutableArray<(String key, String replacement)> _mutableReplacements =
        [.._mutableKeys.Select(static k => (k, k[MutableLength..]))];


    /// <summary>
    /// Adds default replacements to the builder.
    /// </summary>
    /// <remarks>
    /// The following replacements will be added:
    /// <list type="bullet">
    /// <item><c>MutableChildExclusionOptions</c> -> <c>ChildExclusionOptions</c></item>
    /// <item><c>MutableReadmeGeneratorOptions</c> -> <c>ReadmeGeneratorOptions</c></item>
    /// <item><c>MutableRootExclusionOptions</c> -> <c>RootExclusionOptions</c></item>
    /// <item><c>MutableJsonSchemaGeneratorOptions</c> -> <c>JsonSchemaGeneratorOptions</c></item>
    /// <item><c>MutableBindSightGeneratorOptions</c> -> <c>BindSightGeneratorOptions</c></item>
    /// <item><c>MutableBindSightRunnerOptions</c> -> <c>BindSightRunnerOptions</c></item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public KeyReplacementsBuilder UseDefaults()
    {
        foreach ((String key, String replacement) in _mutableReplacements)
            Use(key, replacement);

        return this;
    }

    IReadOnlyDictionary<String, String> IKeyReplacementsProvider.GetReplacements() => _replacements;
}
