namespace RhoMicro.OptionsDocs.Transformations;

using System.Collections.Immutable;
using Models;

/// <summary>
/// Excludes specified properties and generic argument leaves from children.
/// </summary>
/// <param name="options">
/// The options to use when excluding children.
/// </param>
public sealed class ChildExclusionTransformation(IChildExclusionOptions options) : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var children = ImmutableHashSet.CreateBuilder<OptionsModelChild>();

        foreach (var child in model.Children)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (child.GenericArgumentType is not null && !options.ExcludeGenericArgumentLeafs)
            {
                children.Add(child);
            }
            else if (child.Property is { } p && !options.ExcludedProperties.Contains(p))
                children.Add(child);
        }

        var result = model.WithChildren(children.ToImmutable());

        return ValueTask.FromResult<OptionsModel?>(result);
    }
}
