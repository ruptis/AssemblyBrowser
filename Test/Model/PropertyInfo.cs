namespace Test.Model;

public record PropertyInfo(
    string Name,
    string Type,
    IEnumerable<MethodInfo> Accessors) : MemberInfo(Name);
