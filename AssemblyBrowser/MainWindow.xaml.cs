using System.Windows;
using AssemblyBrowser.Command;
using AssemblyBrowser.Service;
using AssemblyBrowser.Store;
using AssemblyBrowser.ViewModel;
namespace AssemblyBrowser;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var assemblyInfoItemsStore = new AssemblyInfoItemsStore(new AssemblyInfoProvider());
        var fileNameStore = new StringStore();
        var dialogService = new DialogService();
        var loadAssemblyCommand = new LoadAssemblyCommand(dialogService, assemblyInfoItemsStore, fileNameStore);
        var mainViewModel = new MainViewModel(assemblyInfoItemsStore, fileNameStore, loadAssemblyCommand);
        DataContext = mainViewModel;
    }
}
