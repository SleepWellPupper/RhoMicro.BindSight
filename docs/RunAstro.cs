#!/usr/local/share/dotnet/dotnet run

#:package Microsoft.Extensions.Logging.Abstractions@10.0.0-preview.5.25277.114
#:package Microsoft.Extensions.Configuration.Abstractions@10.0.0-preview.5.25277.114
#:package Microsoft.Extensions.DependencyInjection.Abstractions@10.0.0-preview.5.25277.114
#:package Microsoft.Extensions.Hosting@10.0.0-preview.5.25277.114
#:package Microsoft.Extensions.Hosting.Abstractions@10.0.0-preview.5.25277.114

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<DotnetWatch>()
    .AddHostedService<NpmRun>();
var host = builder.Build();
await host.RunAsync();

class DotnetWatch(ILogger<DotnetWatch> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var process = new Process();

        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = "watch --project ./ReferenceGenerator/ReferenceGenerator.csproj";

        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        process.ErrorDataReceived += (sender, args) => logger.LogError("{Data}", args.Data);
        process.OutputDataReceived += (sender, args) => logger.LogInformation("{Data}", args.Data);

        process.Start();

        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(ct);
    }
}


class NpmRun(ILogger<NpmRun> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var process = new Process();

        process.StartInfo.FileName = "npm";
        process.StartInfo.Arguments = "run dev --prefix site";

        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        process.ErrorDataReceived += (sender, args) => logger.LogError("{Data}", args.Data);
        process.OutputDataReceived += (sender, args) => logger.LogInformation("{Data}", args.Data);

        process.Start();

        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(ct);
    }
}

