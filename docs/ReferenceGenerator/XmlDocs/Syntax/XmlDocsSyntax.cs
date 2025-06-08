namespace ReferenceGenerator.XmlDocs;

using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization.Formatters;
using System.Xml;

public abstract record XmlDocsSyntax(XmlDocsSyntaxKind Kind)
{
    public abstract void Accept<TVisitor>(TVisitor visitor)
        where TVisitor : IXmlDocsSyntaxVisitor;
}
