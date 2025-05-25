namespace RhoMicro.OptionsDocs.Services;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Fake implementation of <see cref="IHostApplicationLifetime"/>, to be registered as a
/// fallback implementation.
/// </summary>
internal sealed class NullApplicationHostLifetime : IHostApplicationLifetime
{
    /// <inheritdoc />
    public void StopApplication()
    {
    }

    /// <inheritdoc />
    public CancellationToken ApplicationStarted { get; } = CancellationToken.None;

    /// <inheritdoc />
    public CancellationToken ApplicationStopping { get; } = CancellationToken.None;

    /// <inheritdoc />
    public CancellationToken ApplicationStopped { get; } = CancellationToken.None;
}
