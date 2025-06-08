namespace ReferenceGenerator.XmlDocs;

public static class SyntaxFactory
{
    public static DocSyntax Doc(String name, params ReadOnlySpan<MemberSyntax> members)
        => new DocSyntax(
            new AssemblySyntax(
                new NameSyntax(
                    new TextSyntax(name))),
            members is []
                ? MembersSyntax.Empty
                : new MembersSyntax([..members]));

    public static MemberSyntax Member(
        String name,
        params ReadOnlySpan<MainElementSyntax> children)
        => Member(name, null, children);

    public static MemberSyntax Member(
        String name,
        InheritdocSyntax? inheritdoc,
        params ReadOnlySpan<MainElementSyntax> children)
        => new MemberSyntax(name, inheritdoc, [..children]);

    public static MainElementSyntax Summary(String text) => Summary(Text(text));

    public static MainElementSyntax Summary(params ReadOnlySpan<NestedElementSyntax> children)
        => children is []
            ? MainElementSyntax.EmptySummary
            : new MainElementSyntax(
                new SummarySyntax(
                    new NestedElementsOrInheritdocSyntax(
                        new NestedElementsSyntax([..children]))));

    public static MainElementSyntax Summary(InheritdocSyntax inheritdoc)
        => inheritdoc == InheritdocSyntax.Empty
            ? MainElementSyntax.EmptySummary
            : new MainElementSyntax(
                new SummarySyntax(
                    new NestedElementsOrInheritdocSyntax(
                        inheritdoc)));

    public static MainElementSyntax Returns(String text) => Returns(Text(text));

    public static MainElementSyntax Returns(params ReadOnlySpan<NestedElementSyntax> children)
        => children is []
            ? MainElementSyntax.EmptyReturns
            : new MainElementSyntax(
                new ReturnsSyntax(
                    new NestedElementsOrInheritdocSyntax(
                        new NestedElementsSyntax([..children]))));

    public static MainElementSyntax Returns(InheritdocSyntax inheritdoc)
        => inheritdoc == InheritdocSyntax.Empty
            ? MainElementSyntax.InheritedReturns
            : new MainElementSyntax(
                new ReturnsSyntax(
                    new NestedElementsOrInheritdocSyntax(
                        inheritdoc)));

    public static MainElementSyntax Remarks(String text) => Remarks(Text(text));

    public static MainElementSyntax Remarks(params ReadOnlySpan<NestedElementSyntax> children)
        => children is []
            ? MainElementSyntax.EmptyRemarks
            : new MainElementSyntax(
                new RemarksSyntax(
                    new NestedElementsOrInheritdocSyntax(
                        new NestedElementsSyntax([..children]))));

    public static MainElementSyntax Remarks(InheritdocSyntax inheritdoc)
        => inheritdoc == InheritdocSyntax.Empty
            ? MainElementSyntax.InheritedRemarks
            : new MainElementSyntax(
                new RemarksSyntax(
                    new NestedElementsOrInheritdocSyntax(
                        inheritdoc)));

    public static MainElementSyntax Param(String name, String text) => Param(name, Text(text));

    public static MainElementSyntax Param(
        String name,
        params ReadOnlySpan<NestedElementSyntax> children)
        => new MainElementSyntax(
            new ParamSyntax(
                name,
                new NestedElementsOrInheritdocSyntax(
                    new NestedElementsSyntax([..children]))));

    public static MainElementSyntax Param(
        String name,
        InheritdocSyntax inheritdoc)
        => new MainElementSyntax(
            new ParamSyntax(
                name,
                new NestedElementsOrInheritdocSyntax(
                    inheritdoc)));

    public static MainElementSyntax Typeparam(String name, String text) => Typeparam(name, Text(text));

    public static MainElementSyntax Typeparam(
        String name,
        params ReadOnlySpan<NestedElementSyntax> children)
        => new MainElementSyntax(
            new TypeparamSyntax(
                name,
                new NestedElementsOrInheritdocSyntax(
                    new NestedElementsSyntax([..children]))));

    public static MainElementSyntax Typeparam(
        String name,
        InheritdocSyntax inheritdoc)
        => new MainElementSyntax(
            new TypeparamSyntax(
                name,
                new NestedElementsOrInheritdocSyntax(
                    inheritdoc)));

    public static MainElementSyntax Inheritdoc(String cref)
        => cref is []
            ? MainElementSyntax.EmptyInheritdoc
            : new MainElementSyntax(
                new InheritdocSyntax(cref));

    public static NestedElementSyntax Text(String text)
        => text is []
            ? NestedElementSyntax.EmptyText
            : new NestedElementSyntax(
                new TextSyntax(text));

    public static NestedElementSyntax InlineCode(String text) => InlineCode(Text(text));

    public static NestedElementSyntax InlineCode(params ReadOnlySpan<NestedElementSyntax> children)
        => children is []
            ? NestedElementSyntax.EmptyInlineCode
            : new NestedElementSyntax(
                new InlineCodeSyntax(
                    new NestedElementsSyntax([..children])));

    public static NestedElementSyntax BlockCode(String text) => BlockCode(Text(text));

    public static NestedElementSyntax BlockCode(params ReadOnlySpan<NestedElementSyntax> children)
        => children is []
            ? NestedElementSyntax.EmptyBlockCode
            : new NestedElementSyntax(
                new BlockCodeSyntax(
                    new NestedElementsSyntax([..children])));

    public static NestedElementSyntax SeeCref(String cref)
        => new NestedElementSyntax(
            new SeeSyntax(Cref: cref));

    public static NestedElementSyntax SeeHref(String href)
        => new NestedElementSyntax(
            new SeeSyntax(Href: href));

    public static NestedElementSyntax SeeLangword(String langword)
        => new NestedElementSyntax(
            new SeeSyntax(Langword: langword));

    public static NestedElementSyntax Paramref(String name)
        => new NestedElementSyntax(
            new ParamrefSyntax(Name: name));

    public static NestedElementSyntax Typeparamref(String name)
        => new NestedElementSyntax(
            new TypeparamrefSyntax(Name: name));
}
