using AssemblyBrowserLib.Model;
namespace AssemblyBrowser.Service;

public interface IAssemblyInfoProvider
{
    public AssemblyInfo GetAssemblyInfo(string path);
}