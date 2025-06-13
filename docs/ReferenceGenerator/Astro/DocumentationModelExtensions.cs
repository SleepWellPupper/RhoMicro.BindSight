namespace ReferenceGenerator.Astro;

using System.Text;

static class DocumentationModelExtensions
{
    public static String GetMarkdownString(
        this TypeDocumentationModel model,
        AstroReferencePathsContext references,
        Int32 depth = 6)
    {
        var builder = new StringBuilder();
        var visitor = new MarkdownStringVisitor(model.Type, builder, references, model.DocsContext, depth);
        model.Docs.Accept(visitor);
        var result = builder.ToString();
        return result;
    }

    public static String GetMarkdownString(
        this MemberDocumentationModel model,
        AstroReferencePathsContext references,
        Int32 depth = 6)
    {
        var builder = new StringBuilder();
        var visitor = new MarkdownStringVisitor(model.Symbol, builder, references, model.DocsContext, depth);
        model.Docs.Accept(visitor);
        var result = builder.ToString();
        return result;
    }
}
