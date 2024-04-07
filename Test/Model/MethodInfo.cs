namespace Test.Model;

public record MethodInfo(
    string Name,
    string AccessModifier,
    string ReturnType,
    IEnumerable<ParameterInfo> Parameters) : MemberInfo(Name);
