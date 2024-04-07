namespace Test.Model;

public record NamespaceInfo(
    string Name,
    IEnumerable<NamespaceInfo> NestedNamespaces,
    IEnumerable<TypeInfo> Types);
