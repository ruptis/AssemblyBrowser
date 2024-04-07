namespace AssemblyBrowserLib.Model;

public record ConstructorInfo(
    string Name,
    string AccessModifier,
    IReadOnlyList<ParameterInfo> Parameters) : MemberInfo(Name);
