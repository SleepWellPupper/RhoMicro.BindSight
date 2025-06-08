namespace ReferenceGenerator.XmlDocs;

public interface IXmlDocsSyntaxVisitor
{
    void Visit(DocSyntax docSyntax);
    void Visit(AssemblySyntax assemblySyntax);
    void Visit(NameSyntax nameSyntax);
    void Visit(MembersSyntax membersSyntax);
    void Visit(MemberSyntax memberSyntax);

    void Visit(MainElementSyntax mainElementSyntax);

    void Visit(SummarySyntax summarySyntax);
    void Visit(ParamSyntax paramSyntax);
    void Visit(ReturnsSyntax returnsSyntax);
    void Visit(RemarksSyntax remarksSyntax);
    void Visit(TypeparamSyntax typeparamSyntax);

    void Visit(NestedElementsOrInheritdocSyntax nestedElementsOrInheritdocSyntax);
    void Visit(NestedElementsSyntax nestedElementsSyntax);
    void Visit(NestedElementSyntax nestedElementSyntax);

    void Visit(TextSyntax textSyntax);
    void Visit(InlineCodeSyntax inlineCodeSyntax);
    void Visit(BlockCodeSyntax blockCodeSyntax);
    void Visit(SeeSyntax seeSyntax);
    void Visit(ParamrefSyntax paramrefSyntax);
    void Visit(TypeparamrefSyntax typeparamrefSyntax);

    void Visit(InheritdocSyntax inheritdocSyntax);
}
