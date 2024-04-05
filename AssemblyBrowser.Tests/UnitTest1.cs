using AssemblyBrowser.Core;
namespace AssemblyBrowser.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {}

    [Test]
    public void Test1()
    {
        Console.WriteLine(AssemblyInfoExtractor.GetAssemblyInfo("AssemblyBrowser.Core.dll"));
    }
}
