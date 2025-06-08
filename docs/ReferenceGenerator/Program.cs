using ReferenceGenerator;
using ReferenceGenerator.Astro;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddSingleton<AstroDocumentationService>()
    .AddAstroReferenceOptions()
    .AddHostedService<Worker>();

var host = builder.Build();

host.Run();
