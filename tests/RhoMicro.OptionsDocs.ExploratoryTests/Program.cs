using Microsoft.Extensions.Options;
using RhoMicro.HotterReload;
using RhoMicro.OptionsDocs;
using RhoMicro.OptionsDocs.ExploratoryTests;
using RhoMicro.OptionsDocs.Services;
using RhoMicro.OptionsDocs.Transformations;
using HotReloadHandler = RhoMicro.OptionsDocs.ExploratoryTests.HotReloadHandler;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddCommandLine(args).AddInMemoryCollection(new Dictionary<String, String?>()
{
    { "Test:NestedSet:[1]:Int32Property", "1" },
    { "Test:NestedSet:[2]:Int32Property", "2" },
    { "Test:NestedSet:[3]:Int32Property", "3" }
});

builder.Services.AddHostedService<Worker>();

builder.Services
    .AddHotReload()
    .AddHandler<HotReloadHandler>()
    .Services
    .AddDocumentedOptions<TestOptions>("Test")
    .Services.AddOptionsDocs().Generators.AddLogging();

var host = builder.Build();

await host.RunAsync();

namespace RhoMicro.OptionsDocs.ExploratoryTests
{
    using System.Collections.Immutable;
    using RhoMicro.HotterReload;
    using Services;

    sealed class HotReloadHandler(OptionsDocsGeneratorRunner runner) : IHotReloadHandler
    {
        public ValueTask OnHotReload(ImmutableArray<Type> types, CancellationToken ct) => new(runner.Run(ct));
    }

    sealed class TestOptions
    {
        public List<Stream> Streams { get; }
        public List<String> StringList { get; set; } = [];
        public HashSet<TestNestedOptions> NestedSet { get; } = [];
        public TestNestedOptions NestedProperty { get; set; }
    }

    sealed class TestNestedOptions
    {
        public TestNestedOptions CircularReference { get; set; }
        public Int32 Int32Property { get; set; }
    }
}
