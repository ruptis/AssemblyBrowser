namespace Test;

public static class NamespaceExtensions
{
    public static string? GetChildNamespace(this string? fullNamespace, string? parentNamespace) =>
        fullNamespace?.Split('.').SkipWhile(subnamespace => subnamespace != parentNamespace).Skip(1).FirstOrDefault();

    public static bool IsNestedNamespace(this string? fullNamespace, string? parentNamespace) =>
        fullNamespace?.Split('.').SkipWhile(subnamespace => subnamespace != parentNamespace).Skip(1).Any() ?? false;
}
