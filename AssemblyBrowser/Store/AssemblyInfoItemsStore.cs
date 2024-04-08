using System.Windows.Controls;
using AssemblyBrowser.Service;
using AssemblyBrowserLib.Model;
namespace AssemblyBrowser.Store;

public sealed class AssemblyInfoItemsStore(IAssemblyInfoProvider assemblyInfoProvider) : ITreeItemsStore
{
    private readonly List<TreeViewItem> _treeItems = [];

    public IEnumerable<TreeViewItem> TreeItems => _treeItems;
    public event Action? TreeItemsLoaded;
    
    public void Load(string path)
    {
        _treeItems.Clear();
        
        AssemblyInfo assemblyInfo = assemblyInfoProvider.GetAssemblyInfo(path);
        _treeItems.AddRange(assemblyInfo.ToTreeItems());
        
        TreeItemsLoaded?.Invoke();
    }
}
