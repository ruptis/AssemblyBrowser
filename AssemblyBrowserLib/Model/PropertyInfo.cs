namespace AssemblyBrowserLib.Model;

public record PropertyInfo(
    string Name,
    string AccessModifier,
    string Type,
    IReadOnlyList<MethodInfo> Accessors) : MemberInfo(Name);
