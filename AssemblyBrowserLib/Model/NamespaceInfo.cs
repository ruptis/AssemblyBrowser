namespace AssemblyBrowserLib.Model;

public record NamespaceInfo(
    string Name,
    IReadOnlyList<NamespaceInfo> NestedNamespaces,
    IReadOnlyList<TypeInfo> Types);
