namespace ReferenceGenerator;

using Astro;

public class Worker(
    IHostApplicationLifetime lifetime,
    AstroDocumentationService astroDocumentationService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        await astroDocumentationService.Run(ct);

        lifetime.StopApplication();
    }
}
