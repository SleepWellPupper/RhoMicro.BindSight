namespace ReferenceGenerator.Astro;

using System.Text;
using XmlDocs;

internal sealed class HtmlTitleStringVisitor(
    StringBuilder builder,
    XmlDocsContext context)
    : TreeTraversingXmlDocsVisitor
{
    protected override void OnBeforeVisit(TypeparamrefElement typeparamref) => builder.Append(typeparamref.Name);
    protected override void OnBeforeVisit(ParamrefElement paramref) => builder.Append(paramref.Name);
    protected override void OnBeforeVisit(SeeLangwordElement seeLangword) => builder.Append(seeLangword.Langword);

    protected override void OnBeforeVisit(SeeCrefElement seeCref)
    {
        if (!context.SemanticModel.TryGetSymbol(seeCref.Cref, out var symbol))
            return;

        builder.Append(symbol.Name);
    }

    protected override void OnBeforeVisit(TextElement text)
    {
        var start = 0;
        var value = text.Value.AsSpan();
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (c is not ('\n' or '\r' or '"'))
                continue;

            var span = value.Slice(start, i - start);
            builder.Append(span);
            if (c is '"')
                builder.Append('\'');

            start = i + 1;
        }

        if (start != value.Length)
        {
            builder.Append(value[start..]);
        }
    }

    protected override void OnBeforeVisit(InlineCodeElement inlineCode) => builder.Append('\'');
    protected override void OnAfterVisit(InlineCodeElement inlineCode) => builder.Append('\'');
    protected override void OnBeforeVisit(BlockCodeElement blockCode) => builder.Append('\'');
    protected override void OnAfterVisit(BlockCodeElement blockCode) => builder.Append('\'');
}
