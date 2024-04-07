using System.Collections;
using System.Reflection;
using Test.Model;
using MethodInfo = System.Reflection.MethodInfo;
using TypeInfo = Test.Model.TypeInfo;
namespace Test;

public static class AssemblyInfoExtractor
{
    public static AssemblyInfo GetAssemblyInfo(string assemblyPath)
    {
        Assembly assembly = Assembly.LoadFrom(assemblyPath);
        return new AssemblyInfo(
            assembly.GetName().Name ?? "",
            assembly.Location,
            GetNamespaces(assembly, assembly.GetTypes().GetExtensionMethods().ToArray()));
    }

    public static IEnumerable<NamespaceInfo> GetNamespaces(Assembly assembly, IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> extensionMethods) =>
        assembly.GetTypes()
            .GroupBy(type => type.Namespace?.Split('.').First())
            .Select(group => new NamespaceInfo(
                group.Key ?? "",
                GetNestedNamespaces(group, extensionMethods.GetExtensionMethodsForNestedNamespaces(group.Key)),
                GetTypes(group, extensionMethods.GetExtensionMethodsForNamespace(group.Key))));

    private static IEnumerable<NamespaceInfo> GetNestedNamespaces(IGrouping<string?, Type> group, IEnumerable<IGrouping<string?, IGrouping<Type, MethodInfo>>> extensionMethods) =>
        group
            .GroupBy(type => type.Namespace.GetChildNamespace(group.Key))
            .Where(subgroup => subgroup.Key != null)
            .Select(subgroup => new NamespaceInfo(
                subgroup.Key ?? "",
                GetNestedNamespaces(subgroup, extensionMethods.GetExtensionMethodsForNestedNamespaces(subgroup.Key)),
                GetTypes(subgroup, extensionMethods.GetExtensionMethodsForNamespace(subgroup.Key))));

    private static IEnumerable<TypeInfo> GetTypes(IGrouping<string?, Type> group, IEnumerable<IGrouping<Type, MethodInfo>> extensionMethods) =>
        group
            .Where(type => type.Namespace?.EndsWith(group.Key ?? "") == true)
            .Select(type => type.ToTypeInfo(extensionMethods));

}