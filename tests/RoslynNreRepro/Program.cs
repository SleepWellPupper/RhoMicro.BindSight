using Microsoft.CodeAnalysis.MSBuild;

var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync("../../src/RhoMicro.BindSight/RhoMicro.BindSight.csproj");
var compilation = await project.GetCompilationAsync();
compilation?.GetDiagnostics(); // NRE here
