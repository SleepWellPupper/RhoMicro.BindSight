namespace ReferenceGenerator.XmlDocs;

using System.Xml;

public abstract class ParseResult
{
    private ParseResult(IEnumerable<XmlDocsDiagnostic> warnings) => Warnings = warnings.ToList();

    public abstract class Failure : ParseResult
    {
        private Failure(IEnumerable<XmlDocsDiagnostic> warnings)
            : base(warnings)
        {
        }

        public sealed class XmlFailure(XmlException exception) : Failure([])
        {
            public XmlException Exception { get; } = exception;
        }

        public sealed class ParseFailure(
            IEnumerable<XmlDocsDiagnostic> errors,
            IEnumerable<XmlDocsDiagnostic> warnings) : Failure(warnings)
        {
            public IReadOnlyList<XmlDocsDiagnostic> Errors { get; } = errors.ToList();
        }
    }

    public abstract class Success : ParseResult
    {
        private Success(
            XmlDocsSyntax syntax,
            IEnumerable<XmlDocsDiagnostic> warnings)
            : base(warnings)
        {
            Syntax = syntax;
        }

        public virtual XmlDocsSyntax Syntax { get; }

        public sealed class MemberResult(
            MemberSyntax syntax,
            IEnumerable<XmlDocsDiagnostic> warnings)
            : Success(syntax, warnings)
        {
            public override MemberSyntax Syntax { get; } = syntax;
        }

        public sealed class DocResult(
            DocSyntax syntax,
            IEnumerable<XmlDocsDiagnostic> warnings)
            : Success(syntax, warnings)
        {
            public override DocSyntax Syntax { get; } = syntax;
        }
    }

    public IReadOnlyList<XmlDocsDiagnostic> Warnings { get; }
}
