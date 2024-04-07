namespace AssemblyBrowserLib.Tests.TestClasses.Interfaces;

public interface TestGenericInterface<in T> : TestInterface
{
    public void TestMethod(T arg);
}
