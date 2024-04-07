namespace AssemblyBrowserLib.Tests.TestClasses.Extensions;

public static class TestExtensions
{
    public static void TestExtension(this TestClass testClass) { }
    
    public static void TestExtensionWithParameters(this TestClass testClass, int arg1, string arg2) { }
    
    private static void PrivateTestExtension(this TestClass testClass) { }
}
