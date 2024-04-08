using System.Reflection;
using AssemblyBrowserLib;
using AssemblyBrowserLib.Model;
namespace AssemblyBrowser.Service;

public class AssemblyInfoProvider : IAssemblyInfoProvider
{
    public AssemblyInfo GetAssemblyInfo(string path) => 
        Assembly.LoadFrom(path).GetAssemblyInfo();
}
