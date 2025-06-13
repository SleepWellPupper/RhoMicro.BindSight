namespace ReferenceGenerator.XmlDocs;

public abstract record XmlDocsChildElement : XmlDocsElement
{
    internal XmlDocsChildElement(XmlDocsKind Kind) : base(Kind)
    {
    }
    public override String ToString() => base.ToString();
}
