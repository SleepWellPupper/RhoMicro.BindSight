namespace RhoMicro.BindSight.Transformations;

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Enrichments;
using Models;

/// <summary>
/// Adds values specified via <see cref="AllowedValuesAttribute"/>, <see cref="DeniedValuesAttribute"/>
/// and nullability annotations to the models allowed and denied values.
/// </summary>
public sealed class ExplicitlyAllowedValuesTransformation : IOptionsModelTransformation
{
    /// <inheritdoc />
    public ValueTask<OptionsModel?> Transform(OptionsModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var children = ImmutableHashSet.CreateBuilder<OptionsModelChild>();

        var nullabilityInfoContext = new NullabilityInfoContext();

        foreach (var child in model.Children)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var explicitlyDisallowedValues = child
                .GetCustomAttributes<DeniedValuesAttribute>()
                .SelectMany(a => a.Values)
                .ToHashSet();
            var explicitlyAllowedValues = child
                .GetCustomAttributes<AllowedValuesAttribute>()
                .SelectMany(a => a.Values)
                .ToHashSet();

            var previouslyAllowedValues = child.GetAllowedValues();
            var allowedValues = previouslyAllowedValues
                .Except(explicitlyDisallowedValues)
                .Concat(explicitlyAllowedValues).ToHashSet();

            var previouslyDisallowedValues = child.GetDisallowedValues();
            var disallowedValues = previouslyDisallowedValues
                .Except(explicitlyAllowedValues)
                .Concat(explicitlyDisallowedValues).ToHashSet();

            if (child.Property is { } info)
            {
                var nullabilityInfo = nullabilityInfoContext.Create(info);

                if (nullabilityInfo.WriteState is NullabilityState.Nullable && !disallowedValues.Contains(null))
                    allowedValues.Add(null);

                if (nullabilityInfo.WriteState is not NullabilityState.Nullable && !allowedValues.Contains(null))
                    disallowedValues.Add(null);
            }

            var valuesEnrichment = new ValuesEnrichment(
                disallowedValues: disallowedValues,
                allowedValues: allowedValues);
            var newChild = child.WithEnrichment(valuesEnrichment);
            children.Add(newChild);
        }

        var disallowedRootValues = model.Root.GetDisallowedValues().Append(null).ToHashSet();
        var allowedRootValues = model.Root.GetAllowedValues();
        var rootValuesEnrichment = new ValuesEnrichment(
            disallowedValues: disallowedRootValues,
            allowedValues: allowedRootValues);
        var newRoot = model.Root.WithEnrichment(rootValuesEnrichment);

        var result = model.WithRoot(newRoot).WithChildren(children.ToImmutable());

        return ValueTask.FromResult<OptionsModel?>(result);
    }
}
