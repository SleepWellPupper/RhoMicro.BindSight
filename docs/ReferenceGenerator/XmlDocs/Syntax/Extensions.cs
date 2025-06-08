namespace ReferenceGenerator.XmlDocs;

using System.Text;

public static class Extensions
{
    public static String ToXmlString(this XmlDocsSyntax xmlDocsSyntax)
    {
        var builder = new StringBuilder();
        var visitor = new XmlStringVisitor(builder);
        xmlDocsSyntax.Accept(visitor);
        var result = builder.ToString();

        return result;
    }
}
