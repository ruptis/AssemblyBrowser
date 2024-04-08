namespace AssemblyBrowserLib.Model;

public record TypeInfo(
    string Name,
    string AccessModifier,
    IReadOnlyList<string> Modifiers,
    string TypeKind,
    string? BaseType,
    IReadOnlyList<string> ImplementedInterfaces,
    IReadOnlyList<TypeInfo> NestedTypes,
    IReadOnlyList<MemberInfo> Members,
    IReadOnlyList<MethodInfo> ExtensionMethods);
