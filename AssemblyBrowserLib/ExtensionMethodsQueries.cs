using System.Reflection;
using System.Runtime.CompilerServices;
namespace AssemblyBrowserLib;

internal static class ExtensionMethodsQueries
{
    public static IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> GetExtensionMethods(this IEnumerable<Type> typesGroups)
    {
        return typesGroups
            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.IsDefined(typeof(ExtensionAttribute), false))
                .GroupBy(method => method.GetParameters().First().ParameterType))
            .GroupBy(group => group.Key.Namespace);
    }
    
    public static IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> GetExtensionMethodsForNestedNamespaces(
        this IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> extensionMethods,
        string? parentNamespace) =>
        extensionMethods
            .Where(group => group.Key.IsNestedNamespace(parentNamespace));
    
    public static IEnumerable<IGrouping<Type, MethodInfo>> GetExtensionMethodsForNamespace(
        this IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> extensionMethods,
        string? namespaceName) =>
        extensionMethods
            .Where(group => group.Key?.EndsWith(namespaceName ?? "") == true)
            .SelectMany(group => group);
    
    public static IEnumerable<MethodInfo> GetExtensionMethodsForType(
        this IEnumerable<IGrouping<Type, MethodInfo>> extensionMethods,
        Type type) =>
        extensionMethods
            .Where(group => group.Key == type)
            .SelectMany(group => group);
}
