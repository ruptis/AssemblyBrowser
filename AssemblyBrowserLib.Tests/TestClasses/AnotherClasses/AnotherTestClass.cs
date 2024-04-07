using AssemblyBrowserLib.Tests.TestClasses.Interfaces;
namespace AssemblyBrowserLib.Tests.TestClasses.AnotherClasses;

public class AnotherTestClass : TestGenericInterface<int>
{
    public void TestMethod(int arg) { }
    public void TestMethod() { }
}
