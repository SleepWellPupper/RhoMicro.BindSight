namespace ReferenceGenerator.Astro;

internal readonly record struct AstroReferencePaths(
    AnchorHref AnchorHref,
    AbsoluteFilePath AbsoluteFilePath,
    String ContainingDirectory)
{
    public static AstroReferencePaths Empty { get; } = new(
        AnchorHref.Empty,
        AbsoluteFilePath.Empty,
        String.Empty);
}
