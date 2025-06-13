namespace ReferenceGenerator;

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XmlDocs;

internal sealed class TypeDocumentationModel(
    INamedTypeSymbol type,
    XmlDocsContext docsContext)
{
    public INamedTypeSymbol Type => type;
    public MemberElement Docs => DocsContext.Provider.GetXmlDocs(Type);
    public XmlDocsContext DocsContext => docsContext;

    [field: MaybeNull]
    public String Namespace =>
        field ??= Type.ContainingNamespace.ToDisplayString(SymbolDisplayFormats.FullyQualifiedGlobalNamespaceOmitted);

    [field: MaybeNull] public String DisplayName => field ??= type.ToDisplayString(SymbolDisplayFormats.PathName);

    [field: MaybeNull]
    public String Signature => field ??=
        $"{SyntaxFacts.GetText(type.DeclaredAccessibility)} " +
        $"{(type.IsSealed ? "sealed " : type.IsAbstract ? "abstract " : type.IsStatic ? "static " : String.Empty)}" +
        $"{type.ToDisplayString(SymbolDisplayFormats.Signature)}";

    public ImmutableArray<MemberDocumentationModel> Members
    {
        get
        {
            if (field.IsDefault)
                field =
                [
                    .. type.GetMembers()
                        .Where(m =>
                        {
                            // primary ctor or operator?
                            if (m is IMethodSymbol
                                {
                                    MethodKind:
                                    MethodKind.Constructor or
                                    MethodKind.BuiltinOperator or
                                    MethodKind.UserDefinedOperator,
                                    CanBeReferencedByName: false
                                })
                                return true;

                            if (m is
                                {
                                    DeclaredAccessibility: Accessibility.Protected or Accessibility.ProtectedOrInternal,
                                    ContainingType: { IsSealed: true }
                                })
                                return false;

                            if (m.DeclaredAccessibility is not
                                (Accessibility.Protected or
                                Accessibility.ProtectedOrInternal or
                                Accessibility.Public))
                                return false;

                            if (m is not
                                {
                                    Kind:
                                    SymbolKind.Property or
                                    SymbolKind.Field or
                                    SymbolKind.Method or
                                    SymbolKind.Event,
                                })
                                return false;

                            if (m is IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet })
                                return false;

                            return true;
                        })
                        .Select(s => new MemberDocumentationModel(s, docsContext))
                ];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Constructors
    {
        get
        {
            if (field.IsDefault)
                field = [..Members.Where(m => m.Symbol is IMethodSymbol { MethodKind: MethodKind.Constructor })];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Fields
    {
        get
        {
            if (field.IsDefault)
                field = [..Members.Where(m => m.Symbol is IFieldSymbol)];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Properties
    {
        get
        {
            if (field.IsDefault)
                field = [..Members.Where(m => m.Symbol is IPropertySymbol)];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Events
    {
        get
        {
            if (field.IsDefault)
                field = [..Members.Where(m => m.Symbol is IEventSymbol)];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Methods
    {
        get
        {
            if (field.IsDefault)
                field = [..Members.Where(m => m.Symbol is IMethodSymbol { MethodKind: MethodKind.Ordinary })];

            return field;
        }
    }

    public ImmutableArray<MemberDocumentationModel> Operators
    {
        get
        {
            if (field.IsDefault)
                field =
                [
                    ..Members.Where(m => m.Symbol is IMethodSymbol { MethodKind: MethodKind.UserDefinedOperator })
                ];

            return field;
        }
    }

    [field: MaybeNull] public String RecordKeyword => field ??= type.IsRecord ? "record" : String.Empty;

    [field: MaybeNull]
    public String TypeKindKeyword => field ??= type.TypeKind switch
    {
        TypeKind.Class => "class",
        TypeKind.Delegate => "delegate",
        TypeKind.Enum => "enum",
        TypeKind.Interface => "interface",
        TypeKind.Struct => "struct",
        _ => String.Empty
    };


    public override String ToString() => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
}
