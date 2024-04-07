namespace AssemblyBrowserLib.Model;

public record TypeInfo(
    string Name,
    string AccessModifier,
    string BaseType,
    IReadOnlyList<string> ImplementedInterfaces,
    IReadOnlyList<TypeInfo> NestedTypes,
    IReadOnlyList<MemberInfo> Members,
    IReadOnlyList<MethodInfo> ExtensionMethods);
