namespace Test.Model;

public record FieldInfo(
    string Name,
    string AccessModifier,
    string Type) : MemberInfo(Name);
