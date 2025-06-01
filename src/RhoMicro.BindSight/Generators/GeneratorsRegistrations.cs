namespace RhoMicro.BindSight.Generators;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;

/// <summary>
/// Wraps an <see cref="BindSightBuilder"/> for registering options docs generators.
/// </summary>
/// <param name="optionsDocs">
/// The <see cref="BindSightBuilder"/> to wrap.
/// </param>
public sealed class GeneratorsRegistrations(BindSightBuilder optionsDocs)
{
    /// <summary>
    /// Gets the wrapped <see cref="BindSightBuilder"/>.
    /// </summary>
    public BindSightBuilder BindSight => optionsDocs;

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
        where TGenerator : class, IBindSightGenerator
    {
        BindSight.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IBindSightGenerator, TGenerator>());

        return this;
    }

    /// <summary>
    /// Adds the <see cref="LoggingBindSightGenerator"/> for logging all detected options.
    /// </summary>
    /// <returns>
    /// A reference to this object, for chaining of further method calls.
    /// </returns>
    public GeneratorsRegistrations AddLogging() => AddGenerator<LoggingBindSightGenerator>();

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
        BindSight.Services.TryAddJsonSchemaGeneratorOptions(configure);
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
        BindSight.Services.TryAddReadmeGeneratorOptions(configure);
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
