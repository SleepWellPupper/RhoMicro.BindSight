namespace RhoMicro.BindSight.Transformations;

using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Models;

/// <summary>
/// Sequentially executes transformations on a list of option models.
/// </summary>
/// <param name="transformations">
/// The sequence of transformations to execute.
/// </param>
/// <param name="logger">
/// The logger to use.
/// </param>
public sealed class TransformationPipeline(
    IEnumerable<IOptionsModelTransformation> transformations,
    ILogger<TransformationPipeline> logger)
{
    private readonly ImmutableArray<IOptionsModelTransformation> _transformations = [..transformations];

    /// <summary>
    /// Transforms a list of options models.
    /// </summary>
    /// <param name="options">
    /// The options to transform.
    /// </param>
    /// <param name="ct">
    /// The cancellation token used to request pipeline execution to be cancelled.
    /// </param>
    /// <returns>
    /// A task, that, upon completion, will contain the resulting list of models.
    /// </returns>
    public async ValueTask<ImmutableArray<OptionsModel>> TransformOptions(
        ImmutableArray<OptionsModel> options,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = ImmutableArray.CreateBuilder<OptionsModel>(options.Length);

        try
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Beginning transformations in order: {TransformationPipeline}",
                    String.Join("->", _transformations.Select(t => t.GetType().Name)));
            }

            await Parallel.ForAsync(0, options.Length, ct, (i, ct) => TransformMapping(i, options, result, ct));

            logger.LogInformation("Done with transformations.");
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            logger.LogInformation("TransformationPipeline were cancelled.");
        }

        return result.DrainToImmutable();
    }

    private async ValueTask TransformMapping(
        Int32 i,
        ImmutableArray<OptionsModel> mappings,
        ImmutableArray<OptionsModel>.Builder result,
        CancellationToken ct)
    {
        var mapping = mappings[i];

        foreach (var transformation in _transformations)
        {
            ct.ThrowIfCancellationRequested();

            var task = ApplyTransformation(mapping, transformation, ct);

            if (!task.IsCompletedSuccessfully)
                await task;

            mapping = task.Result;

            if (mapping is null)
                return;
        }

        lock (result)
            result.Add(mapping);
    }

    private async ValueTask<OptionsModel?> ApplyTransformation(
        OptionsModel model,
        IOptionsModelTransformation transformation,
        CancellationToken ct)
    {
        try
        {
            var task = transformation.Transform(model, ct);

            if (!task.IsCompletedSuccessfully)
                await task;

            return task.Result;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(
                ex,
                "Error while executing transformation '{Transformation}'. The transformation has been ignored.",
                transformation);
            return model;
        }
    }
}
