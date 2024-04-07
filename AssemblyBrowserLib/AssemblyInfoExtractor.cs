using System.Reflection;
using AssemblyBrowserLib.Model;
using MethodInfo = System.Reflection.MethodInfo;
using TypeInfo = AssemblyBrowserLib.Model.TypeInfo;
namespace AssemblyBrowserLib;

public static class AssemblyInfoExtractor
{
    public static AssemblyInfo GetAssemblyInfo(this Assembly assembly) => new(
        assembly.GetName().Name ?? "",
        assembly.Location,
        assembly.GetNamespaces(assembly.GetTypes().GetExtensionMethods().ToArray()));

    private static NamespaceInfo[] GetNamespaces(this Assembly assembly, IGrouping<string?, IGrouping<Type, MethodInfo>>[] extensionMethods) =>
        assembly.GetTypes()
            .GroupBy(type => type.Namespace?.Split('.').First())
            .Select(group => new NamespaceInfo(
                group.Key ?? "",
                group.GetNestedNamespaces(extensionMethods.GetExtensionMethodsForNestedNamespaces(group.Key).ToArray()),
                group.GetTypes(extensionMethods.GetExtensionMethodsForNamespace(group.Key).ToArray())))
            .ToArray();

    private static NamespaceInfo[] GetNestedNamespaces(this IGrouping<string?, Type> group, IGrouping<string?, IGrouping<Type, MethodInfo>>[] extensionMethods) =>
        group
            .GroupBy(type => type.Namespace.GetChildNamespace(group.Key))
            .Where(subgroup => subgroup.Key != null)
            .Select(subgroup => new NamespaceInfo(
                subgroup.Key ?? "",
                GetNestedNamespaces(subgroup, extensionMethods.GetExtensionMethodsForNestedNamespaces(subgroup.Key).ToArray()),
                GetTypes(subgroup, extensionMethods.GetExtensionMethodsForNamespace(subgroup.Key).ToArray())))
            .ToArray();

    private static TypeInfo[] GetTypes(this IGrouping<string?, Type> group, IGrouping<Type, MethodInfo>[] extensionMethods) =>
        group
            .Where(type => type.Namespace?.EndsWith(group.Key ?? "") == true && !type.IsNested)
            .Select(type => type.ToTypeInfo(extensionMethods))
            .ToArray();

}
