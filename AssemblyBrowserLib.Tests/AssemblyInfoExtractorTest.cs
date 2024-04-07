using System.Reflection;
using AssemblyBrowserLib.Model;
using ConstructorInfo = AssemblyBrowserLib.Model.ConstructorInfo;
using FieldInfo = AssemblyBrowserLib.Model.FieldInfo;
using MethodInfo = AssemblyBrowserLib.Model.MethodInfo;
using PropertyInfo = AssemblyBrowserLib.Model.PropertyInfo;
using TypeInfo = AssemblyBrowserLib.Model.TypeInfo;
namespace AssemblyBrowserLib.Tests;

public class AssemblyInfoExtractorTest
{
    private AssemblyInfo _assemblyInfo = null!;

    [SetUp]
    public void Setup()
    {
        _assemblyInfo = Assembly.GetExecutingAssembly().GetAssemblyInfo();
    }

    [Test]
    public void TestAssemblyName()
    {
        Assert.That(_assemblyInfo.Name, Is.EqualTo("AssemblyBrowserLib.Tests"));
    }

    [Test]
    public void TestRootNamespace()
    {
        Assert.That(_assemblyInfo.Namespaces, Has.Count.EqualTo(2));
        NamespaceInfo ns = HasNamespace(_assemblyInfo.Namespaces, "AssemblyBrowserLib");
        ns = HasNamespace(ns.NestedNamespaces, "Tests");
        Assert.That(ns.NestedNamespaces, Has.Count.EqualTo(1));
    }

    [Test]
    public void TestNestedNamespace()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        Assert.That(ns.NestedNamespaces, Has.Count.EqualTo(3));
    }

    [Test]
    public void TestClass()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        Assert.That(ns.Types, Has.Count.EqualTo(2));
        TypeInfo type = HasType(ns.Types, "TestClass");
        Assert.That(type.NestedTypes, Has.Count.EqualTo(1));
        Assert.That(type.Members, Has.Count.EqualTo(22));
        Assert.That(type.NestedTypes, Has.Count.EqualTo(1));
    }

    [Test]
    public void TestExtensionMethods()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        Assert.That(type.ExtensionMethods, Has.Count.EqualTo(3));
    }

    [Test]
    public void TestNestedType()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        TypeInfo nestedType = HasType(type.NestedTypes, "NestedClass");
        Assert.That(nestedType.Members, Has.Count.EqualTo(8));
    }

    [Test]
    public void TestExtensionMethod()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        MethodInfo extensionMethod = type.ExtensionMethods[0];
        Assert.Multiple(() =>
        {
            Assert.That(extensionMethod.Name, Is.EqualTo("TestExtension"));
            Assert.That(extensionMethod.Parameters, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void TestMethod()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        MethodInfo method = type.Members.OfType<MethodInfo>().First(m => m.Name == "PublicMethod");
        Assert.Multiple(() =>
        {
            Assert.That(method.Name, Is.EqualTo("PublicMethod"));
            Assert.That(method.Parameters, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void TestMethodWithParameters()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        MethodInfo method = type.Members.OfType<MethodInfo>().First(m => m.Name == "MethodWithParameters");
        Assert.Multiple(() =>
        {
            Assert.That(method.Name, Is.EqualTo("MethodWithParameters"));
            Assert.That(method.Parameters, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void TestMethodWithGeneric()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        MethodInfo method = type.Members.OfType<MethodInfo>().First(m => m.Name == "MethodWithGeneric");
        Assert.Multiple(() =>
        {
            Assert.That(method.Name, Is.EqualTo("MethodWithGeneric"));
            Assert.That(method.Parameters, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void TestField()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        FieldInfo field = type.Members.OfType<FieldInfo>().First(f => f.Name == "PublicField");
        Assert.That(field.Name, Is.EqualTo("PublicField"));
    }

    [Test]
    public void TestProperty()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        PropertyInfo property = type.Members.OfType<PropertyInfo>().First(p => p.Name == "PublicProperty");
        Assert.That(property.Name, Is.EqualTo("PublicProperty"));
    }

    [Test]
    public void TestConstructor()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        TypeInfo type = HasType(ns.Types, "TestClass");
        ConstructorInfo constructor = type.Members.OfType<ConstructorInfo>().First();
        Assert.That(constructor.Name, Is.EqualTo(".ctor"));
    }

    [Test]
    public void TestInterface()
    {
        NamespaceInfo ns = HasTestClassesNamespace();
        ns = HasNamespace(ns.NestedNamespaces, "Interfaces");
        TypeInfo type = HasType(ns.Types, "TestInterface");
        Assert.That(type.Members, Has.Count.EqualTo(1));
        TypeInfo genericType = HasType(ns.Types, "TestGenericInterface");
        Assert.That(genericType.Members, Has.Count.EqualTo(1));
        Assert.That(genericType.ImplementedInterfaces, Has.Count.EqualTo(1));
    }
    
    private NamespaceInfo HasTestClassesNamespace()
    {
        NamespaceInfo ns = HasNamespace(_assemblyInfo.Namespaces, "AssemblyBrowserLib");
        ns = HasNamespace(ns.NestedNamespaces, "Tests");
        ns = HasNamespace(ns.NestedNamespaces, "TestClasses");
        return ns;
    }

    private static NamespaceInfo HasNamespace(IEnumerable<NamespaceInfo> namespaces, string name)
    {
        NamespaceInfo? ns = namespaces.FirstOrDefault(n => n.Name == name);
        Assert.That(ns, Is.Not.Null);
        return ns!;
    }

    private static TypeInfo HasType(IEnumerable<TypeInfo> types, string name)
    {
        TypeInfo? type = types.FirstOrDefault(t => t.Name == name);
        Assert.That(type, Is.Not.Null);
        return type!;
    }
}
