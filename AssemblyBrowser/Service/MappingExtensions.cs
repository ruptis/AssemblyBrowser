using System.Windows;
using System.Windows.Controls;
using AssemblyBrowserLib.Model;
namespace AssemblyBrowser.Service;

public static class MappingExtensions
{
    public static IEnumerable<TreeViewItem> ToTreeItems(this AssemblyInfo assemblyInfo) =>
        assemblyInfo.Namespaces.Select(namespaceInfo => namespaceInfo.ToTreeItem());

    private static TreeViewItem ToTreeItem(this NamespaceInfo namespaceInfo) => new()
    {
        Header = namespaceInfo.Name,
        ItemsSource = namespaceInfo.NestedNamespaces.Select(info => info.ToTreeItem())
            .Concat(namespaceInfo.Types.Select(info => info.ToTreeItem())),
        Style = Application.Current.FindResource("NamespaceInfoStyle") as Style
    };

    private static TreeViewItem ToTreeItem(this TypeInfo typeInfo) => new()
    {
        Header = GetTypeHeader(typeInfo),
        ItemsSource = typeInfo.NestedTypes.Select(info => info.ToTreeItem())
            .Concat(typeInfo.Members.Select(info => info.ToTreeItem()))
            .Concat(typeInfo.ExtensionMethods.Select(info => info.ToTreeItem())),
        Style = Application.Current.FindResource("TypeInfoStyle") as Style
    };

    private static string GetTypeHeader(TypeInfo typeInfo) =>
        typeInfo.AccessModifier + " " + string.Join(" ", typeInfo.Modifiers) + " " + typeInfo.TypeKind + " " + typeInfo.Name +
        (typeInfo.BaseType is not null ? " : " + typeInfo.BaseType : "") +
        (typeInfo.ImplementedInterfaces.Count > 0 ? typeInfo.BaseType is not null ? ", " : " : " + string.Join(", ", typeInfo.ImplementedInterfaces) : "");

    private static TreeViewItem ToTreeItem(this MemberInfo memberInfo) => new()
    {
        Header = GetMemberHeader(memberInfo),
    };

    private static string GetMemberHeader(MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            FieldInfo fieldInfo => GetFieldHeader(fieldInfo),
            PropertyInfo propertyInfo => GetPropertyHeader(propertyInfo),
            MethodInfo methodInfo => GetMethodHeader(methodInfo),
            _ => memberInfo.Name
        };
    }

    private static string GetFieldHeader(FieldInfo fieldInfo) =>
        fieldInfo.AccessModifier + " " + fieldInfo.Type + " " + fieldInfo.Name;

    private static string GetPropertyHeader(PropertyInfo propertyInfo) =>
        propertyInfo.AccessModifier + " " + propertyInfo.Type + " " + propertyInfo.Name +
        " { " + string.Join(", ", propertyInfo.Accessors.Select(info => info.Name)) + " }";

    private static string GetMethodHeader(MethodInfo methodInfo) =>
        methodInfo.AccessModifier + " " + methodInfo.ReturnType + " " +
        (methodInfo.GenericArguments.Count > 0 ? "<" + string.Join(", ", methodInfo.GenericArguments) + ">" : "") +
        " " + methodInfo.Name +
        "(" + string.Join(", ", methodInfo.Parameters.Select(info => info.Type + " " + info.Name)) + ")";
}
