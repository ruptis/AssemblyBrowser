using System.Reflection;
using System.Text;
namespace AssemblyBrowser.Core;

public static class AssemblyInfoExtractor
{
    public static AssemblyInfo GetAssemblyInfo(string assemblyPath)
    {
        Assembly assembly = Assembly.LoadFrom(assemblyPath);
        return new AssemblyInfo(assembly.GetName().Name ?? "", assembly.Location, GetNamespaces(assembly));
    }
    
    private static NamespaceInfo[] GetNamespaces(Assembly assembly)
    {
        return assembly.GetTypes()
            .GroupBy(type => type.Namespace)
            .Select(group => new NamespaceInfo(group.Key ?? "", GetTypes(group.ToArray())))
            .ToArray();
    }
    
    private static TypeInfo[] GetTypes(Type[] types)
    {
        return types
            .Select(type => new TypeInfo(type.Name, type.IsPublic ? "public" : "private", type.BaseType?.Name ?? "", type.GetInterfaces().Select(i => i.Name).ToArray(), GetMembers(type)))
            .ToArray();
    }
    
    private static MemberInfo[] GetMembers(Type type)
    {
        return type.GetMembers()
            .Select(MemberInfo (member) => member.MemberType switch
            {
                MemberTypes.Constructor => GetConstructorInfo((System.Reflection.ConstructorInfo)member),
                MemberTypes.Field => GetFieldInfo((System.Reflection.FieldInfo)member),
                MemberTypes.Property => GetPropertyInfo((System.Reflection.PropertyInfo)member),
                MemberTypes.Method => GetMethodInfo((System.Reflection.MethodInfo)member),
                _ => new MemberInfo(member.Name)
            })
            .ToArray();
    }
    
    private static ConstructorInfo GetConstructorInfo(System.Reflection.ConstructorInfo constructor) => 
        new(
            constructor.Name, 
            constructor.IsPublic ? "public" : "private", 
            constructor.GetParameters().Select(parameter => parameter.ParameterType.Name).ToArray());

    private static FieldInfo GetFieldInfo(System.Reflection.FieldInfo field) => 
        new(
            field.Name, 
            field.IsPublic ? "public" : "private", 
            field.FieldType.Name);

    private static PropertyInfo GetPropertyInfo(System.Reflection.PropertyInfo property) => 
        new(
            property.Name, 
            property.PropertyType.Name, 
            property.GetAccessors().Select(GetMethodInfo).ToArray());

    private static MethodInfo GetMethodInfo(System.Reflection.MethodInfo method) => 
        new(
            method.Name,
            method.IsPublic ? "public" : "private",
            method.ReturnType.Name, 
            method.GetParameters().Select(parameter => parameter.ParameterType.Name).ToArray());
}

public record AssemblyInfo(string Name, string Location, NamespaceInfo[] Namespaces)
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Assembly: {Name}");
        sb.AppendLine($"Location: {Location}");
        foreach (NamespaceInfo ns in Namespaces)
            sb.AppendLine($"Namespace: {ns}");
        return sb.ToString();
    }
}

public record NamespaceInfo(string Name, TypeInfo[] Types)
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Namespace: {Name}");
        foreach (TypeInfo type in Types)
            sb.AppendLine($"Type: {type}");
        return sb.ToString();
    }
}

public record TypeInfo(string Name, string AccessModifier, string BaseType, string[] ImplementedInterfaces, MemberInfo[] Members)
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Type: {AccessModifier} {Name}");
        if (BaseType != "")
            sb.AppendLine($"Base type: {BaseType}");
        if (ImplementedInterfaces.Length > 0)
            sb.AppendLine($"Implemented interfaces: {string.Join(", ", ImplementedInterfaces)}");
        foreach (MemberInfo member in Members)
            sb.AppendLine($"Member: {member}");
        return sb.ToString();
    }

}

public record MemberInfo(string Name);

public record FieldInfo(string Name, string AccessModifier, string Type) : MemberInfo(Name);

public record PropertyInfo(string Name, string Type, MethodInfo[] Accessors) : MemberInfo(Name);

public record ConstructorInfo(string Name, string AccessModifier, string[] Parameters) : MemberInfo(Name);

public record MethodInfo(string Name, string AccessModifier, string ReturnType, string[] Parameters) : MemberInfo(Name);

