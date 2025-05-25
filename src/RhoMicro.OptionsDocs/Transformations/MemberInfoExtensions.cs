namespace RhoMicro.OptionsDocs.Transformations;

using System.ComponentModel;
using System.Reflection;

internal static class MemberInfoExtensions
{
    public static String GetDescription(this MemberInfo memberInfo)
        => memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? String.Empty;
}