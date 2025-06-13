namespace ReferenceGenerator;

using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;

internal static class SymbolExtensions
{
    public static String GetMemberName(this ISymbol symbol) =>
        symbol switch
        {
            IMethodSymbol
            {
                MethodKind: MethodKind.Constructor,
                CanBeReferencedByName: false
            } => symbol.ContainingType
                .Name,
            IMethodSymbol
            {
                MethodKind: MethodKind.BuiltinOperator or MethodKind.UserDefinedOperator
            } op => op.Name switch
            {
                "op_Decrement" => "--",
                "op_Increment" => "++",
                "op_UnaryNegation" or "op_Subtraction" => "-",
                "op_UnaryPlus" or "op_Addition" => "+",
                "op_LogicalNot" => "!",
                "op_AddressOf" or "op_BitwiseAnd" => "&",
                "op_OnesComplement" => "~",
                "op_PointerDereference" or "op_Multiply" => "*",
                "op_Division" => "/",
                "op_Modulus" => "%",
                "op_ExclusiveOr" => "^",
                "op_BitwiseOr" => "|",
                "op_LogicalAnd" => "&&",
                "op_LogicalOr" => "||",
                "op_LeftShift" => "<<",
                "op_RightShift" => ">>",
                "op_Equality" => "==",
                "op_GreaterThan" => ">",
                "op_LessThan" => "<",
                "op_Inequality" => "!=",
                "op_GreaterThanOrEqual" => ">=",
                "op_LessThanOrEqual" => "<=",
                "op_MemberSelection" => "->",
                "op_RightShiftAssignment" => ">>=",
                "op_MultiplicationAssignment" => "*=",
                "op_PointerToMemberSelection" => "->*",
                "op_SubtractionAssignment" => "-=",
                "op_ExclusiveOrAssignment" => "^=",
                "op_LeftShiftAssignment" => "<<=",
                "op_ModulusAssignment" => "%=",
                "op_AdditionAssignment" => "+=",
                "op_BitwiseAndAssignment" => "&=",
                "op_BitwiseOrAssignment" => "|=",
                "op_DivisionAssignment" => "/=",
                _ => op.Name
            },
            _ => symbol.Name
        };
}
