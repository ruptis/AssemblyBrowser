namespace Test.Model;

public record ConstructorInfo(
    string Name,
    string AccessModifier,
    IEnumerable<ParameterInfo> Parameters) : MemberInfo(Name);
