namespace RhoMicro.OptionsDocs.Transformations;

using System.Collections.Immutable;
using Enrichments;
using Models;

/// <summary>
/// Adds the values <see langword="true"/> and <see langword="false"/> to the allowed
/// values for children whose modelled type is <see cref="Boolean"/> or <see cref="Nullable{Boolean}"/>.
/// </summary>
public sealed class AllowedBooleanValuesTransformation : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var children = ImmutableHashSet.CreateBuilder<OptionsModelChild>();

        foreach (var child in model.Children)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (child.ModelledType != typeof(Boolean) && child.ModelledType != typeof(Boolean?))
            {
                children.Add(child);
                continue;
            }

            var allowedValues = child.GetAllowedValues().Append(true).Append(false).ToHashSet();
            var valuesEnrichment = new ValuesEnrichment(
                disallowedValues: child.GetDisallowedValues(),
                allowedValues: allowedValues);
            var newChild = child.WithEnrichment(valuesEnrichment);
            children.Add(newChild);
        }

        var result = model.WithChildren(children.ToImmutable());

        return ValueTask.FromResult<OptionsModel?>(result);
    }
}
