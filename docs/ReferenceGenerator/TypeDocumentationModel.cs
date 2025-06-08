namespace ReferenceGenerator;

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XmlDocs;

internal sealed class TypeDocumentationModel(
    INamedTypeSymbol type,
    Compilation compilation,
    XmlDocsParseNodeOptions xmlDocsParseNodeOptions)
{
    public INamedTypeSymbol Type => type;

    [field: MaybeNull]
    public String Namespace =>
        field ??= Type.ContainingNamespace.ToDisplayString(SymbolDisplayFormats.FullyQualifiedNamespaceOmitted);

    [field: MaybeNull] public String TypeKindDisplay => field ??= type.TypeKind.ToString();

    [field: MaybeNull]
    public MemberNode Documentation =>
        field ??= XmlDocsParser.Create(
            type,
            compilation,
            xmlDocsParseNodeOptions).Member;

    [field: MaybeNull] public String DisplayName => field ??= type.ToDisplayString(SymbolDisplayFormats.PathName);

    [field: MaybeNull]
    public String Signature => field ??=
        $"{SyntaxFacts.GetText(type.DeclaredAccessibility)} {type.ToDisplayString(SymbolDisplayFormats.Signature)}";

    public ImmutableArray<MemberDocumentationModel> Members
    {
        get
        {
            if (field.IsDefault)
                field =
                [
                    .. type.GetMembers()
                        .Where(m => m is
                            {
                                DeclaredAccessibility: Accessibility.Protected or Accessibility.Public,
                                Kind: SymbolKind.Property or SymbolKind.Field or SymbolKind.Method or SymbolKind.Event
                            }
                            and not IMethodSymbol
                            {
                                MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet
                            }
                        )
                        .Select(s => new MemberDocumentationModel(s, compilation, xmlDocsParseNodeOptions))
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
