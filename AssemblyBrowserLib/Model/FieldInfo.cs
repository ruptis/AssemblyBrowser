namespace AssemblyBrowserLib.Model;

public record FieldInfo(
    string Name,
    string AccessModifier,
    string Type) : MemberInfo(Name);
