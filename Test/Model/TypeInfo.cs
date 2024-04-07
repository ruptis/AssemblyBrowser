namespace Test.Model;

public record TypeInfo(
    string Name,
    string AccessModifier,
    string BaseType,
    IEnumerable<string> ImplementedInterfaces,
    IEnumerable<TypeInfo> NestedTypes,
    IEnumerable<MemberInfo> Members,
    IEnumerable<MethodInfo> ExtensionMethods);
