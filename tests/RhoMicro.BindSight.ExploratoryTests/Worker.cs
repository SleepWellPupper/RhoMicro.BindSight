namespace RhoMicro.BindSight.ExploratoryTests;

public class Worker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Delay(Timeout.Infinite, stoppingToken);
}
