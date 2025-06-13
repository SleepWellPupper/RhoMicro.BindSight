namespace ReferenceGenerator;

using Microsoft.CodeAnalysis;

internal static class SymbolDisplayFormats
{
    public static SymbolDisplayFormat FullyQualifiedGlobalNamespaceOmitted { get; } =
        SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

    public static SymbolDisplayFormat FullyQualifiedNamespaceOmitted { get; } =
        new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

    public static SymbolDisplayFormat Constraints { get; } =
        new(genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeConstraints);

    public static SymbolDisplayFormat HeaderSignature { get; } =
        new(genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            parameterOptions: SymbolDisplayParameterOptions.IncludeType,
            memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public static SymbolDisplayFormat NameOrSpecialName { get; } =
        new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public static SymbolDisplayFormat SimpleGenericName { get; } =
        new(memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

    public static SymbolDisplayFormat Signature { get; } =
        new(globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeVariance |
                             SymbolDisplayGenericsOptions.IncludeTypeConstraints |
                             SymbolDisplayGenericsOptions.IncludeTypeParameters,
            delegateStyle: SymbolDisplayDelegateStyle.NameAndSignature,
            memberOptions: SymbolDisplayMemberOptions.IncludeAccessibility |
                           SymbolDisplayMemberOptions.IncludeModifiers |
                           SymbolDisplayMemberOptions.IncludeParameters |
                           SymbolDisplayMemberOptions.IncludeRef |
                           SymbolDisplayMemberOptions.IncludeType |
                           SymbolDisplayMemberOptions.IncludeConstantValue |
                           SymbolDisplayMemberOptions.IncludeExplicitInterface,
            parameterOptions: SymbolDisplayParameterOptions.IncludeType |
                              SymbolDisplayParameterOptions.IncludeModifiers |
                              SymbolDisplayParameterOptions.IncludeName |
                              SymbolDisplayParameterOptions.IncludeDefaultValue |
                              SymbolDisplayParameterOptions.IncludeExtensionThis |
                              SymbolDisplayParameterOptions.IncludeOptionalBrackets,
            extensionMethodStyle: SymbolDisplayExtensionMethodStyle.StaticMethod,
            kindOptions: SymbolDisplayKindOptions.IncludeMemberKeyword |
                         SymbolDisplayKindOptions.IncludeNamespaceKeyword |
                         SymbolDisplayKindOptions.IncludeTypeKeyword,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes |
                                  SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier |
                                  SymbolDisplayMiscellaneousOptions.UseErrorTypeSymbolName,
            propertyStyle: SymbolDisplayPropertyStyle.ShowReadWriteDescriptor);

    public static SymbolDisplayFormat PathName { get; } =
        new(globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            delegateStyle: SymbolDisplayDelegateStyle.NameOnly);

    public static SymbolDisplayFormat ExternalReferenceFormat { get; } =
        FullyQualifiedGlobalNamespaceOmitted.WithMemberOptions(
            SymbolDisplayMemberOptions.IncludeContainingType);
}
