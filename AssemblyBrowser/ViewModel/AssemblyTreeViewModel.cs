using System.Windows.Controls;
using AssemblyBrowser.Store;
namespace AssemblyBrowser.ViewModel;

public sealed class AssemblyTreeViewModel : BaseViewModel
{
    private readonly ITreeItemsStore _treeItemsStore;
    
    public AssemblyTreeViewModel(ITreeItemsStore treeItemsStore)
    {
        _treeItemsStore = treeItemsStore;
        _treeItemsStore.TreeItemsLoaded += OnTreeItemsLoaded;
    }
    
    private List<TreeViewItem> _treeItems = [];
    public List<TreeViewItem> TreeItems
    {
        get => _treeItems;
        private set => SetField(ref _treeItems, value);
    }

    private void OnTreeItemsLoaded() => 
        TreeItems = _treeItemsStore.TreeItems.ToList();
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _treeItemsStore.TreeItemsLoaded -= OnTreeItemsLoaded;
        }
    }
}
