namespace AssemblyBrowserLib.Tests.TestClasses;

public record struct TestRecordStruct
{
    public int TestField;
    public int TestProperty { get; set; }
    public int TestMethod() => 0;
}
