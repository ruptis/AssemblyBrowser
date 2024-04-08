using System.Reflection;
using ConstructorInfo = AssemblyBrowserLib.Model.ConstructorInfo;
using FieldInfo = AssemblyBrowserLib.Model.FieldInfo;
using MemberInfo = AssemblyBrowserLib.Model.MemberInfo;
using MethodInfo = AssemblyBrowserLib.Model.MethodInfo;
using ParameterInfo = AssemblyBrowserLib.Model.ParameterInfo;
using PropertyInfo = AssemblyBrowserLib.Model.PropertyInfo;
using TypeInfo = AssemblyBrowserLib.Model.TypeInfo;
namespace AssemblyBrowserLib;
internal static class MappingExtensions
{
    public static TypeInfo ToTypeInfo(this Type type, IGrouping<Type, System.Reflection.MethodInfo>[] extensionMethods) => new(
        type.GetFullName(),
        type.GetAccessModifier(),
        type.GetModifiers(),
        type.GetTypeKind(),
        type.BaseType?.GetFullName(),
        type.GetInterfacesNames(),
        type.GetNestedTypes(extensionMethods).ToArray(),
        type.GetMembersInfo().ToArray(),
        extensionMethods.GetExtensionMethodsForType(type)
            .Select(method => method.ToMethodInfo())
            .ToArray());

    private static string GetAccessModifier(this Type type) =>
        type.IsPublic ? "public" : type.IsNestedPublic ? "public" : type.IsNestedPrivate ? "private" : type.IsNestedFamily ? "protected" : "internal";
    
    private static List<string> GetModifiers(this Type type)
    {
        var modifiers = new List<string>();
        if (type.IsStatic())
        {
            modifiers.Add("static");
            return modifiers;
        }
        
        if (type is { IsAbstract: true, IsInterface: false })
            modifiers.Add("abstract");
        
        if (type.IsSealed)
            modifiers.Add("sealed");
        
        return modifiers;
    }
    
    private static string GetTypeKind(this Type type) => 
        type.IsInterface ? "interface" : type.IsEnum ? "enum" : type.IsValueType ? "struct" : "class";
    
    private static bool IsStatic(this Type type) => type is { IsAbstract: true, IsSealed: true };
    
    private static string GetFullName(this Type type) =>
        type.IsGenericType ? type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(arg => arg.Name)) + ">" : type.Name;

    private static string[] GetInterfacesNames(this Type type) =>
        type.GetInterfaces().Select(interf => interf.IsGenericType ? interf.Name.Split('`')[0] : interf.Name).ToArray();
    
    private static IEnumerable<TypeInfo> GetNestedTypes(this Type type, IGrouping<Type, System.Reflection.MethodInfo>[] extensionMethods) =>
        type.GetNestedTypes().Select(nestedType => nestedType.ToTypeInfo(extensionMethods));
    
    private static IEnumerable<MemberInfo> GetMembersInfo(this Type type) =>
        type.GetRuntimeFields().Select<System.Reflection.FieldInfo, MemberInfo>(field => field.ToFieldInfo())
            .Concat(type.GetRuntimeProperties().Select(property => property.ToPropertyInfo()))
            .Concat(type.GetConstructors().Select(constructor => constructor.ToConstructorInfo()))
            .Concat(type.GetRuntimeMethods().Where(method => !method.IsSpecialName).Select(method => method.ToMethodInfo()));

    private static MethodInfo ToMethodInfo(this System.Reflection.MethodInfo methodInfo) => new(
        methodInfo.Name,
        methodInfo.GetAccessModifier(),
        methodInfo.ReturnType.GetFullName(),
        methodInfo.GetGenericArgumentsNames(),
        methodInfo.GetParameters().Select(parameter => parameter.ToParameterInfo()).ToArray());
    
    private static string GetAccessModifier(this MethodBase methodInfo) => 
        methodInfo.IsPublic ? "public" : methodInfo.IsPrivate ? "private" : methodInfo.IsFamily ? "protected" : "internal";
    
    private static List<string> GetGenericArgumentsNames(this System.Reflection.MethodInfo methodInfo) =>
        methodInfo.IsGenericMethod ? methodInfo.GetGenericArguments().Select(arg => arg.GetFullName()).ToList() : [];

    private static ParameterInfo ToParameterInfo(this System.Reflection.ParameterInfo parameterInfo) => new(
        parameterInfo.GetModifier(),
        parameterInfo.ParameterType.GetFullName(),
        parameterInfo.Name ?? "");
    
    private static string GetModifier(this System.Reflection.ParameterInfo parameterInfo) => 
        parameterInfo.IsOut ? "out" : parameterInfo.IsIn ? "in" : parameterInfo.IsRetval ? "ref" : "";

    private static FieldInfo ToFieldInfo(this System.Reflection.FieldInfo fieldInfo) => new(
        fieldInfo.Name,
        fieldInfo.GetAccessModifier(),
        fieldInfo.FieldType.GetFullName());
    
    private static string GetAccessModifier(this System.Reflection.FieldInfo fieldInfo) => 
        fieldInfo.IsPublic ? "public" : fieldInfo.IsPrivate ? "private" : fieldInfo.IsFamily ? "protected" : "internal";

    private static PropertyInfo ToPropertyInfo(this System.Reflection.PropertyInfo propertyInfo) => new(
        propertyInfo.Name,
        propertyInfo.GetAccessModifier(),
        propertyInfo.PropertyType.GetFullName(),
        new[]
            {
                propertyInfo.GetGetMethod(),
                propertyInfo.GetSetMethod()
            }
            .Where(accessor => accessor != null)
            .Select(accessor => accessor!.ToMethodInfo())
            .ToArray());
    
    private static string GetAccessModifier(this System.Reflection.PropertyInfo propertyInfo) => 
        propertyInfo.GetMethod?.IsPublic ?? false ? "public" : propertyInfo.GetMethod?.IsPrivate ?? false ? "private" : propertyInfo.GetMethod?.IsFamily ?? false ? "protected" : "internal";

    private static ConstructorInfo ToConstructorInfo(this System.Reflection.ConstructorInfo constructorInfo) => new(
        constructorInfo.Name,
        constructorInfo.GetAccessModifier(),
        constructorInfo.GetParameters().Select(parameter => parameter.ToParameterInfo()).ToArray());
}
