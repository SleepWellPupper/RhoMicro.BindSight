# OptionsDocs

## TODO

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

## Architecture

- 5 main areas:
    - Services
    - Models
    - Enrichments
    - Transformations
    - Generators

### Services

- responsible for integration of various components and services
- provides top level integration point into DI via `ServiceCollectionExtensions.AddOptionsDocs(this IServiceCollection)`

### Models

- models option registrations and their properties
- create models via:
    - `OptionsModel.Create(Type type, EnrichmentFactory enrichmentFactory, OptionsModelCreationOptions options)`
    -

### Enrichments

-
