using System.Windows.Input;
using AssemblyBrowser.Store;
namespace AssemblyBrowser.ViewModel;

public sealed class MainViewModel : BaseViewModel
{
    private readonly IStringStore _fileNameStore;

    public MainViewModel(ITreeItemsStore treeItemsStore, IStringStore fileNameStore, ICommand openCommand)
    {
        AssemblyTreeViewModel = new AssemblyTreeViewModel(treeItemsStore);
        OpenCommand = openCommand;
        _fileNameStore = fileNameStore;
        _fileNameStore.StringChanged += OnFileNameChanged;
    }

    private void OnFileNameChanged() => FileName = _fileNameStore.String;

    public AssemblyTreeViewModel AssemblyTreeViewModel { get; }

    private string _fileName = string.Empty;
    public string FileName
    {
        get => _fileName;
        private set
        {
            _fileName = value;
            OnPropertyChanged();
        }
    }

    public ICommand OpenCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _fileNameStore.StringChanged -= OnFileNameChanged;
            AssemblyTreeViewModel.Dispose();
        }
    }
}
