namespace AssemblyBrowserLib.Model;

public record PropertyInfo(
    string Name,
    string Type,
    IReadOnlyList<MethodInfo> Accessors) : MemberInfo(Name);
