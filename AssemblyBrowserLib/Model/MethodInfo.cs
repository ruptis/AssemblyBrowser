namespace AssemblyBrowserLib.Model;

public record MethodInfo(
    string Name,
    string AccessModifier,
    string ReturnType,
    IReadOnlyList<string> GenericArguments,
    IReadOnlyList<ParameterInfo> Parameters) : MemberInfo(Name);
