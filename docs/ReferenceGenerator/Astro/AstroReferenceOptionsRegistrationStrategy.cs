namespace ReferenceGenerator.Astro;

using Microsoft.Extensions.Options;

partial class AstroReferenceOptionsRegistrationStrategy
{
    partial class Pattern<T>
    {
        static partial void ConfigureOptionsBuilder(
            OptionsBuilder<MutableAstroReferenceOptions> builder,
            AstroReferenceOptionsConfiguration configuration)
        {
            builder.ValidateDataAnnotations();
        }
    }
}
