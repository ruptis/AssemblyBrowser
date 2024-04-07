namespace AssemblyBrowserLib.Tests.TestClasses;

public class TestClass
{
    public class NestedClass
    {
        public void NestedMethod() {}
    }

    public float PublicField;
    private int _privateField;
    public string PublicProperty { get; set; }
    private string PrivateProperty { get; set; }
    public string PublicGetOnlyProperty { get; }

    public TestClass() {}

    public TestClass(int arg) {}

    public void PublicMethod() {}
    private void PrivateMethod() {}

    public void MethodWithParameters(int arg1, string arg2) {}

    public void MethodWithGeneric<T>(T arg) {}

    public void MethodWithGenericAndParameters<T>(T arg1, T arg2) {}

    public void MethodWithGenericAndConstraints<T>(T arg) where T : class {}
}
