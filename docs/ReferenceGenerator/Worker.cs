namespace ReferenceGenerator;

using Astro;

internal class Worker(
    ApplicationLifetime lifetime,
    AstroDocumentationService astroDocumentationService,
    ILogger<Worker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        try
        {
            await astroDocumentationService.Run(ct);
            lifetime.StopApplication();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while running astro service.");
            lifetime.StopApplication(-1);
        }
    }
}
