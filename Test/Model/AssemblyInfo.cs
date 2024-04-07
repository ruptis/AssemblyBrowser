namespace Test.Model;

public record AssemblyInfo(string Name, string Location, IEnumerable<NamespaceInfo> Namespaces);
