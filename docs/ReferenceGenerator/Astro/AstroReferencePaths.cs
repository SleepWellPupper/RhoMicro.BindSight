namespace ReferenceGenerator.Astro;

internal readonly record struct AstroReferencePaths(
    String AnchorHref,
    String AbsoluteFilePath,
    String ContainingDirectory,
    String HeadingId)
{
    public static AstroReferencePaths Empty { get; } = new(
        String.Empty,
        String.Empty,
        String.Empty,
        String.Empty);
}
