namespace ReferenceGenerator.XmlDocs;

using System.Text;

public interface IStringBuildingXmlDocsVisitor : IXmlDocsVisitor
{
    public static abstract IXmlDocsVisitor Create(StringBuilder builder);
}
