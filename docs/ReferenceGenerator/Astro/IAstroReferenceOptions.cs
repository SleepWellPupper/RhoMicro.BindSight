namespace ReferenceGenerator.Astro;

using System.ComponentModel.DataAnnotations;
using RhoMicro.CodeAnalysis;

[Options]
public partial interface IAstroReferenceOptions
{
    [DeniedValues([null], ErrorMessage = $"{nameof(ProjectPath)} cannot be null.")]
    String ProjectPath { get; }

    [DeniedValues([null], ErrorMessage = $"{nameof(ReferenceDirectory)} cannot be null.")]
    String ReferenceDirectory { get; }

    [DefaultValueExpression("String.Empty")]
    String RelativeReferencePathBase { get; }

    [DefaultValueExpression("true")] Boolean ClearReferenceDirectory { get; }

    [DefaultValueExpression(@"""https://duckduckgo.com/?q={0}""")]
    String ExternalReferenceUrlFormat { get; }

    [DefaultValueExpression("true")] Boolean DryRun { get; }
}
