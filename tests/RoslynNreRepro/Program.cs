using Microsoft.CodeAnalysis.MSBuild;

var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync("../RoslynNreReproTarget/RoslynNreReproTarget.csproj");
var compilation = await project.GetCompilationAsync();
compilation?.GetDiagnostics(); // NRE here
