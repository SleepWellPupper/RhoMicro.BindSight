namespace ReferenceGenerator.Astro;

using System.Text;
using ReferenceGenerator.XmlDocs;
using RhoMicro.CodeAnalysis.Library.Text.Templating;

internal readonly struct AstroDocumentationNodeTemplate(
    XmlDocsParserNode node,
    AstroReferencePathContext referencePaths)
    : ITemplate
{
    public void Render<TBody>(ref TemplateRenderer renderer, TBody body, CancellationToken cancellationToken)
        where TBody : ITemplate
    {
        var builder = new StringBuilder();
        node.Accept(new AstroXmlDocsVisitor(builder, referencePaths), cancellationToken);
        var text = builder.ToString().Trim();
        renderer.Render(text);
    }
}
