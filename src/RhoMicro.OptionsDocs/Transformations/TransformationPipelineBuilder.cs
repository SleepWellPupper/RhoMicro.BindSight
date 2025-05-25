namespace RhoMicro.OptionsDocs.Transformations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;

/// <summary>
/// Builds the sequence of transformations used in <see cref="TransformationPipeline"/>.
/// </summary>
/// <param name="optionsDocs">
/// The options docs builder to wrap.
/// </param>
public sealed class TransformationPipelineBuilder(OptionsDocsBuilder optionsDocs) : ITransformationsProvider
{
    private readonly Object _lock = new();
    private Boolean _isReadonly;
    private readonly List<Func<IServiceProvider, IOptionsModelTransformation>> _transformations = [];

    /// <summary>
    /// Gets the wrapped options docs builder.
    /// </summary>
    public OptionsDocsBuilder OptionsDocs => optionsDocs;

    /// <summary>
    /// Gets a builder for configuring key replacements.
    /// </summary>
    public KeyReplacementsBuilder KeyReplacements { get; } = new(optionsDocs);

    IEnumerable<IOptionsModelTransformation> ITransformationsProvider.GetTransformations(
        IServiceProvider serviceProvider)
    {
        if (!_isReadonly)
        {
            lock (_lock)
                SetReadOnly();
        }

        var result = _transformations.Select(f => f.Invoke(serviceProvider)).ToList();

        return result;
    }

    /// <summary>
    /// Clears all transformations from the builder.
    /// </summary>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public TransformationPipelineBuilder Clear()
    {
        EnsureNotReadOnly();
        lock (_lock)
        {
            EnsureNotReadOnly();
            _transformations.Clear();
        }

        return this;
    }

    /// <summary>
    /// Appends a transformation to the transformations sequence.
    /// </summary>
    /// <param name="factory">
    /// The factory callback using which to create the transformation.
    /// </param>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public TransformationPipelineBuilder Use(Func<IServiceProvider, IOptionsModelTransformation> factory)
    {
        EnsureNotReadOnly();
        lock (_lock)
        {
            EnsureNotReadOnly();
            _transformations.Add(factory);
        }

        return this;
    }

    /// <summary>
    /// Appends a transformation to the transformations sequence.
    /// </summary>
    /// <typeparam name="TTransformation">
    /// The type of transformation to append.
    /// </typeparam>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public TransformationPipelineBuilder Use<TTransformation>()
        where TTransformation : class, IOptionsModelTransformation
    {
        optionsDocs.Services.AddSingleton<TTransformation>();
        return Use(static sp => sp.GetRequiredService<TTransformation>());
    }

    /// <summary>
    /// Attempts to append default transformations to the transformation sequence.
    /// </summary>
    /// <remarks>
    /// The default transformations are (in order of execution):
    /// <list type="number">
    /// <item><see cref="RootExclusionTransformation"/></item>
    /// <item><see cref="ChildExclusionTransformation"/></item>
    /// <item><see cref="ChildlessExclusionTransformation"/></item>
    /// <item><see cref="KeyReplacementTransformation"/></item>
    /// <item><see cref="AllowedEnumValuesTransformation"/></item>
    /// <item><see cref="AllowedBooleanValuesTransformation"/></item>
    /// <item><see cref="ExplicitlyAllowedValuesTransformation"/></item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A reference to the builder, for chaining of further method calls.
    /// </returns>
    public TransformationPipelineBuilder UseDefaults()
    {
        optionsDocs.Services.TryAddSingleton<RootExclusionTransformation>();
        optionsDocs.Services.TryAddRootExclusionOptions(c => c.UseCustomOptions(_ => new RootExclusionOptions()));
        optionsDocs.Services.TryAddSingleton<ChildExclusionTransformation>();
        optionsDocs.Services.TryAddChildExclusionOptions(c => c.UseCustomOptions(_ => new ChildExclusionOptions()));
        optionsDocs.Services.TryAddSingleton<KeyReplacementTransformation>();
        optionsDocs.Services.TryAddSingleton<IKeyReplacementsProvider>(KeyReplacements);
        optionsDocs.Services.TryAddSingleton<AllowedEnumValuesTransformation>();
        optionsDocs.Services.TryAddSingleton<AllowedBooleanValuesTransformation>();
        optionsDocs.Services.TryAddSingleton<ChildlessExclusionTransformation>();
        optionsDocs.Services.TryAddSingleton<ExplicitlyAllowedValuesTransformation>();

        KeyReplacements.UseDefaults();

        EnsureNotReadOnly();
        lock (_lock)
        {
            EnsureNotReadOnly();
            _transformations.Add(static sp => sp.GetRequiredService<RootExclusionTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<ChildExclusionTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<ChildlessExclusionTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<KeyReplacementTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<AllowedEnumValuesTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<AllowedBooleanValuesTransformation>());
            _transformations.Add(static sp => sp.GetRequiredService<ExplicitlyAllowedValuesTransformation>());
        }

        return this;
    }

    private void SetReadOnly() => _isReadonly = true;

    private void EnsureNotReadOnly()
    {
        if (_isReadonly)
            throw new InvalidOperationException("Unable to add transformations after the first retrieval.");
    }
}
