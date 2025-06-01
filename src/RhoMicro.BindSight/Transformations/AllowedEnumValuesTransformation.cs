namespace RhoMicro.BindSight.Transformations;

using System.Collections.Immutable;
using Enrichments;
using Models;

/// <summary>
/// Adds allowed enum values to children whose modelled type is an enum type.
/// </summary>
public sealed class AllowedEnumValuesTransformation : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var children = ImmutableHashSet.CreateBuilder<OptionsModelChild>();

        foreach (var child in model.Children)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (EnumValue.Create(child.ModelledType) is not { Length: > 0 } values)
            {
                children.Add(child);
                continue;
            }

            var allowedValues = child.GetAllowedValues().Concat(values).ToHashSet();
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
