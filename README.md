# BindSight

BindSight generates documentation for your [configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration) bindings using the [options pattern](https://learn.microsoft.com/en-us/dotnet/core/extensions/options).

## Installation

Using the command line:

```
dotnet add package RhoMicro.BindSight
```

Using msbuild:

```xml
<PackageReference Include="RhoMicro.BindSight" Version="*"></PackageReference>
```

## Getting Started

Check out the [documentation](https://bindsight.rhomicro.com/tutorials/walkthrough/) on how to get started.

## TODO

- file about enrichments

- generate arbitrary mdx files
- generate graphviz/plantuml diagrams

- generated mdx file marker for directory cleaning

- roslyn bug (NRE in ubuntu)

- nuget

- json schema generator should emit for model instead of root type

- rider issue with live updating file when dotnet watching from out of process
- rider issue with meta update handler not being invoked when debugging

- architecture outline
- design choices & tradeoffs incurred
- showcase website
- fork some popular app repos and integrate to test & show off
    - https://github.com/davidfowl/TriviaR
    - https://github.com/dotnet/eShop
    - https://github.com/dotnet/aspire
- add unit tests
- write a user guide for usage & extension
- cookbook on various usecases
- in use guides, highlight built in transformations & generators & options

- add convenience methods for registering and configuring stuff like excluded children etc

