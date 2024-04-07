namespace AssemblyBrowserLib.Model;

public record AssemblyInfo(string Name, string Location, IReadOnlyList<NamespaceInfo> Namespaces);
