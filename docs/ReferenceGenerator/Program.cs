using ReferenceGenerator;
using ReferenceGenerator.Astro;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddSingleton<ApplicationLifetime>()
    .AddSingleton<AstroDocumentationService>()
    .AddAstroReferenceOptions()
    .AddHostedService<Worker>();

var host = builder.Build();

var lifetime = host.Services.GetRequiredService<ApplicationLifetime>();

await host.RunAsync();

return lifetime.ExitCode;
