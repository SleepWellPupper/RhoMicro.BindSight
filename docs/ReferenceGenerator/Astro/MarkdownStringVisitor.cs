namespace ReferenceGenerator.Astro;

using System.Text;
using Microsoft.CodeAnalysis;
using XmlDocs;

internal sealed class MarkdownStringVisitor(
    ISymbol targetSymbol,
    StringBuilder builder,
    AstroReferencePathsContext referencePaths,
    XmlDocsContext docsContext,
    Int32 depth)
    : TreeTraversingXmlDocsVisitor
{
    protected override void OnBeforeVisit(XmlDocsElements<ExceptionElement> exceptions)
        => builder.Append('#', depth).AppendLine(" Exceptions").AppendLine();

    protected override void Visit(XmlDocsElements<ExceptionElement> exceptions)
    {
        if (exceptions is [])
            return;

        base.Visit(exceptions);
    }

    protected override void OnBeforeVisit(XmlDocsElements<ParamElement> @params)
        => builder.Append('#', depth)
            .AppendLine(" Parameters")
            .AppendLine()
            .AppendLine("| Name | Type | Description |")
            .AppendLine("| ---- | ---- | ----------- |");

    protected override void Visit(XmlDocsElements<ParamElement> @params)
    {
        if (@params is [] || targetSymbol is not IMethodSymbol)
            return;

        base.Visit(@params);
    }

    protected override void OnBeforeVisit(XmlDocsElements<TypeparamElement> typeparams)
        => builder.Append('#', depth)
            .AppendLine(" Type Parameters")
            .AppendLine()
            .AppendLine("| Name | Description | Constraints |")
            .AppendLine("| ---- | ----------- | ----------- |");

    protected override void Visit(XmlDocsElements<TypeparamElement> typeparams)
    {
        if (typeparams is [])
            return;

        base.Visit(typeparams);
    }

    protected override void OnBeforeVisit(ExceptionElement exception)
    {
        if (!docsContext.SemanticModel.TryGetSymbol(exception.Cref, out var symbol))
            return;

        builder.Append('#', depth + 1).Append(' ');
        AppendSymbolReference(symbol);
        builder.AppendLine().AppendLine();
    }

    protected override void OnBeforeVisit(ParamElement param)
    {
        builder.Append("""| <span style="white-space: nowrap">`""").Append(param.Name).Append("`</span> | ");


        if (targetSymbol is IMethodSymbol { Parameters: [_, ..] parameters } &&
            parameters.FirstOrDefault(p => p.Name == param.Name) is { Type: { } paramType })
        {
            AppendSymbolReference(paramType);
        }

        builder.Append(" | ");
    }

    protected override void OnAfterVisit(ParamElement param)
        => builder.AppendLine(" |");

    protected override void OnBeforeVisit(TypeparamElement typeparam)
        => builder.Append($"| `{typeparam.Name}` | ");

    protected override void OnAfterVisit(TypeparamElement typeparam)
    {
        builder.Append(" | ");

        if ((targetSymbol switch
            {
                INamedTypeSymbol type => type.TypeParameters,
                IMethodSymbol method => [..method.TypeParameters, ..method.ContainingType.TypeParameters],
                _ => []
            }).FirstOrDefault(p => p.Name == typeparam.Name) is { } typeParameter)
        {
            var firstConstraint = true;
            var classType = typeParameter.ConstraintTypes.FirstOrDefault(t => t.TypeKind is TypeKind.Class);
            if (classType is not null)
            {
                firstConstraint = false;
                AppendSymbolReference(classType);
            }

            if (typeParameter switch
                {
                    { HasReferenceTypeConstraint: true } => "`class`",
                    { HasNotNullConstraint: true } => "`notnull`",
                    { HasUnmanagedTypeConstraint: true } => "`unmanaged`",
                    { HasValueTypeConstraint: true } => "`struct`",
                    _ => null
                } is { } primaryConstraint)
            {
                if (!firstConstraint)
                    builder.Append(", ");

                firstConstraint = false;
                builder.Append(primaryConstraint);
            }

            foreach (var constraint in typeParameter.ConstraintTypes)
            {
                if (constraint is not { TypeKind: not TypeKind.Class } secondaryConstraint)
                    continue;

                if (!firstConstraint)
                    builder.Append(", ");

                firstConstraint = false;
                AppendSymbolReference(secondaryConstraint);
            }

            if (typeParameter.HasConstructorConstraint)
            {
                if (!firstConstraint)
                    builder.Append(", ");

                builder.Append("`new()`");
            }
        }

        builder.AppendLine(" |");
    }

    public override void Visit(SummaryElement summary)
    {
        if (summary.Elements is [])
            return;

        base.Visit(summary);
    }

    protected override void OnBeforeVisit(SummaryElement summary)
    {
        builder
            .Append('#', depth)
            .AppendLine($" Summary")
            .AppendLine();
    }

    protected override void OnAfterVisit(SummaryElement summary) => builder.AppendLine().AppendLine();

    public override void Visit(RemarksElement remarks)
    {
        if (remarks.Elements is [])
            return;


        base.Visit(remarks);
    }

    protected override void OnBeforeVisit(RemarksElement remarks)
    {
        builder
            .Append('#', depth)
            .AppendLine($" Remarks")
            .AppendLine();
    }

    protected override void OnAfterVisit(RemarksElement remarks) => builder.AppendLine().AppendLine();

    public override void Visit(ReturnsElement returns)
    {
        if (returns.Elements is [])
            return;

        base.Visit(returns);
    }

    protected override void OnBeforeVisit(ReturnsElement returns)
    {
        builder
            .Append('#', depth)
            .AppendLine($" Returns")
            .AppendLine();
    }

    protected override void OnAfterVisit(ReturnsElement returns) => builder.AppendLine().AppendLine();

    public override void Visit(ExampleElement example)
    {
        if (example.Elements is [])
            return;

        base.Visit(example);
    }

    protected override void OnBeforeVisit(ExampleElement example)
    {
        builder
            .Append('#', depth)
            .AppendLine($" Example")
            .AppendLine();
    }

    protected override void OnAfterVisit(ExampleElement example) => builder.AppendLine().AppendLine();

    protected override void OnBeforeVisit(TextElement text)
        => builder.Append(text.Value);

    protected override void OnBeforeVisit(TypeparamrefElement typeparamref)
    {
        if ((targetSymbol switch
            {
                INamedTypeSymbol type => type.TypeParameters,
                IMethodSymbol method => [..method.TypeParameters, ..method.ContainingType.TypeParameters],
                _ => []
            }).FirstOrDefault(p => p.Name == typeparamref.Name) is { } typeparam)
        {
            AppendSymbolReference(typeparam);
        }
        else
        {
            builder.Append($"`{typeparamref.Name}`");
        }
    }

    protected override void OnBeforeVisit(ParamrefElement paramref)
    {
        if (targetSymbol is IMethodSymbol { Parameters: [_, ..] parameters } &&
            parameters.FirstOrDefault(p => p.Name == paramref.Name) is { } param)
        {
            AppendSymbolReference(param);
        }
        else
        {
            builder.Append($"`{paramref.Name}`");
        }
    }

    protected override void OnBeforeVisit(SeeLangwordElement seeLangword)
        => builder.Append($"`{seeLangword.Langword}`");

    protected override void OnBeforeVisit(SeeHrefElement seeHref)
        => builder.Append($"[{seeHref.Href}]({seeHref.Href})");

    protected override void OnBeforeVisit(SeeCrefElement seeCref) => AppendSymbolReference(seeCref.Cref);

    protected override void OnBeforeVisit(ParaElement para)
        => builder.AppendLine().AppendLine();

    protected override void OnAfterVisit(ParaElement para)
        => builder.AppendLine().AppendLine();

    protected override void OnBeforeVisit(BlockCodeElement blockCode)
        => builder.AppendLine().AppendLine("```cs");

    protected override void OnAfterVisit(BlockCodeElement blockCode)
        => builder.AppendLine().AppendLine("```");

    protected override void OnBeforeVisit(InlineCodeElement inlineCode)
        => builder.Append('`');

    protected override void OnAfterVisit(InlineCodeElement inlineCode)
        => builder.Append('`');

    private void AppendSymbolReference(String id)
    {
        if (!docsContext.SemanticModel.TryGetSymbol(id, out var symbol))
            return;

        AppendSymbolReference(symbol);
    }

    private void AppendSymbolReference(ISymbol symbol)
        => builder.AppendMarkdownLink(symbol, docsContext, referencePaths);
}
