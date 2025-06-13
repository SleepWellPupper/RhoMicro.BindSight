namespace ReferenceGenerator.XmlDocs;

using System.Text;

public abstract record XmlDocsElement
{
    internal XmlDocsElement(XmlDocsKind Kind)
    {
        this.Kind = Kind;
    }

    public abstract void Accept<TVisitor>(TVisitor visitor)
        where TVisitor : IXmlDocsVisitor;

    public XmlDocsKind Kind { get; }

    public String ToString<TVisitor>()
        where TVisitor : IStringBuildingXmlDocsVisitor
    {
        var builder = new StringBuilder();
        var visitor = TVisitor.Create(builder);
        Accept(visitor);
        var result = builder.ToString();
        return result;
    }

    public override String ToString() => ToString<CommentStringVisitor>();
}
