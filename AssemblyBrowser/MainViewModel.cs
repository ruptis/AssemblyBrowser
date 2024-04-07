using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
namespace AssemblyBrowser;

public class MainViewModel : INotifyPropertyChanged
{
    public MainViewModel()
    {
        // Test data
        var item = new TreeViewItem
        {
            Header = "Test"
        };
        TreeItems.Add(new TreeViewItem
        {
            Header = "Test"
        });
        TreeItems.Add(item);
        TreeItems.Add(new TreeViewItem
        {
            Header = "Test"
        });

        item.Items.Add(new TreeViewItem
        {
            Header = "Test"
        });
        
        item.Items.Add(new TreeViewItem
        {
            Header = "Test"
        });
        
        item.Items.Add(new TreeViewItem
        {
            Header = "Test"
        });
    }

    private List<TreeViewItem> _treeItems = [];
    public List<TreeViewItem> TreeItems
    {
        get => _treeItems;
        set => SetField(ref _treeItems, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
