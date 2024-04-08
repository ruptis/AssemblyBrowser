using System.Windows.Controls;
namespace AssemblyBrowser.Store;

public interface ITreeItemsStore
{
    public IEnumerable<TreeViewItem> TreeItems { get; }
    public event Action? TreeItemsLoaded;
}
