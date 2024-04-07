namespace AssemblyBrowserLib.Model;

public record MethodInfo(
    string Name,
    string AccessModifier,
    string ReturnType,
    IReadOnlyList<ParameterInfo> Parameters) : MemberInfo(Name);
