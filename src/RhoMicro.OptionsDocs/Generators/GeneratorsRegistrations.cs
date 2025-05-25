namespace RhoMicro.OptionsDocs.Generators;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;

/// <summary>
/// Wraps an <see cref="OptionsDocsBuilder"/> for registering options docs generators.
/// </summary>
/// <param name="optionsDocs">
/// The <see cref="OptionsDocsBuilder"/> to wrap.
/// </param>
public sealed class GeneratorsRegistrations(OptionsDocsBuilder optionsDocs)
{
    /// <summary>
    /// Gets the wrapped <see cref="OptionsDocsBuilder"/>.
    /// </summary>
    public OptionsDocsBuilder OptionsDocs => optionsDocs;

    /// <summary>
    /// Adds a generator to generate docs with.
    /// </summary>
    /// <typeparam name="TGenerator">
    /// The type of generator to register.
    /// </typeparam>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddGenerator<TGenerator>()
        where TGenerator : class, IOptionsDocsGenerator
    {
        OptionsDocs.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IOptionsDocsGenerator, TGenerator>());

        return this;
    }

    /// <summary>
    /// Adds the <see cref="LoggingOptionsDocsGenerator"/> for logging all detected options.
    /// </summary>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddLogging() => AddGenerator<LoggingOptionsDocsGenerator>();

    /// <summary>
    /// Adds the <see cref="JsonSchemaGenerator"/> for generating json schemata of
    /// options.
    /// </summary>
    /// <param name="configure">
    /// <see cref="ServiceCollectionExtensions.TryAddJsonSchemaGeneratorOptions(IServiceCollection, Action{JsonSchemaGeneratorOptionsConfiguration})"/>
    /// </param>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddJsonSchemata(
        Action<JsonSchemaGeneratorOptionsConfiguration>? configure = null)
    {
        OptionsDocs.Services.TryAddJsonSchemaGeneratorOptions(configure);
        return AddGenerator<JsonSchemaGenerator>();
    }

    /// <summary>
    /// Adds the <see cref="ReadmeGenerator"/> for generating a readme of options.
    /// </summary>
    /// <param name="configure">
    /// <see cref="ServiceCollectionExtensions.TryAddReadmeGeneratorOptions(IServiceCollection, Action{ReadmeGeneratorOptionsConfiguration})"/>
    ///  </param>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddReadme(Action<ReadmeGeneratorOptionsConfiguration>? configure = null)
    {
        OptionsDocs.Services.TryAddReadmeGeneratorOptions(configure);
        return AddGenerator<ReadmeGenerator>();
    }

    /// <summary>
    /// Adds default generators.
    /// </summary>
    /// <remarks>
    /// The generators added are:
    /// <list type="bullet">
    /// <item><see cref="ReadmeGenerator"/></item>
    /// <item><see cref="JsonSchemaGenerator"/></item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddDefaults()
        => AddReadme().AddJsonSchemata();
}
