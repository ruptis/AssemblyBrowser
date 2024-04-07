using Test.Model;
namespace Test;

public static class MappingExtensions
{
    public static MethodInfo ToMethodInfo(this System.Reflection.MethodInfo methodInfo) => new(
        methodInfo.Name,
        methodInfo.IsPublic ? "public" : methodInfo.IsPrivate ? "private" : methodInfo.IsFamily ? "protected" : "internal",
        methodInfo.ReturnType.Name,
        methodInfo.GetParameters().Select(parameter => parameter.ToParameterInfo()).ToArray());

    public static ParameterInfo ToParameterInfo(this System.Reflection.ParameterInfo parameterInfo) => new(
        parameterInfo.IsOut ? "out" : parameterInfo.IsIn ? "in" : parameterInfo.IsRetval ? "ref" : "",
        parameterInfo.ParameterType.Name,
        parameterInfo.Name ?? "");

    public static TypeInfo ToTypeInfo(this Type type, IEnumerable<IGrouping<Type, System.Reflection.MethodInfo>> extensionMethods) => new(
        type.Name,
        type.GetAccessModifier(),
        type.BaseType?.Name ?? "",
        type.GetInterfacesNames(),
        type.GetNestedTypes(extensionMethods),
        type.GetMembersInfo().ToArray(),
        extensionMethods.GetExtensionMethodsForType(type)
            .Select(method => method.ToMethodInfo()));

    private static string GetAccessModifier(this Type type) =>
        type.IsPublic ? "public" : type.IsNestedPublic ? "public" : type.IsNestedPrivate ? "private" : type.IsNestedFamily ? "protected" : "internal";

    private static string[] GetInterfacesNames(this Type type) =>
        type.GetInterfaces().Select(interf => interf.Name).ToArray();

    private static IEnumerable<TypeInfo> GetNestedTypes(this Type type, IEnumerable<IGrouping<Type, System.Reflection.MethodInfo>> extensionMethods) =>
        type.GetNestedTypes().Select(nestedType => nestedType.ToTypeInfo(extensionMethods));

    private static IEnumerable<MemberInfo> GetMembersInfo(this Type type) =>
        type.GetFields().Select<System.Reflection.FieldInfo, MemberInfo>(field => field.ToFieldInfo())
            .Concat(type.GetProperties().Select(property => property.ToPropertyInfo()))
            .Concat(type.GetConstructors().Select(constructor => constructor.ToConstructorInfo()))
            .Concat(type.GetMethods().Where(method => !method.IsSpecialName).Select(method => method.ToMethodInfo()));

    public static FieldInfo ToFieldInfo(this System.Reflection.FieldInfo fieldInfo) => new(
        fieldInfo.Name,
        fieldInfo.IsPublic ? "public" : fieldInfo.IsPrivate ? "private" : fieldInfo.IsFamily ? "protected" : "internal",
        fieldInfo.FieldType.Name);

    public static PropertyInfo ToPropertyInfo(this System.Reflection.PropertyInfo propertyInfo) => new(
        propertyInfo.Name,
        propertyInfo.PropertyType.Name,
        new[]
            {
                propertyInfo.GetGetMethod(),
                propertyInfo.GetSetMethod()
            }
            .Where(accessor => accessor != null)
            .Select(accessor => accessor!.ToMethodInfo())
            .ToArray());

    public static ConstructorInfo ToConstructorInfo(this System.Reflection.ConstructorInfo constructorInfo) => new(
        constructorInfo.Name,
        constructorInfo.IsPublic ? "public" : constructorInfo.IsPrivate ? "private" : constructorInfo.IsFamily ? "protected" : "internal",
        constructorInfo.GetParameters().Select(parameter => parameter.ToParameterInfo()).ToArray());
}
