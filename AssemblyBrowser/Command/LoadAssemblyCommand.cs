using System.Windows.Input;
using AssemblyBrowser.Service;
using AssemblyBrowser.Store;
namespace AssemblyBrowser.Command;

public sealed class LoadAssemblyCommand(IDialogService dialogService, AssemblyInfoItemsStore assemblyInfoItemsStore, StringStore stringStore) : ICommand
{
    private readonly OpenFileDialogSettings _settings = new(
        "Select assembly file",
        "Assembly files (*.dll, *.exe)|*.dll;*.exe|All files (*.*)|*.*",
        Environment.CurrentDirectory
    );

    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter)
    {
        var path = dialogService.OpenFileDialog(_settings);
        if (path == null)
            return;
        
        stringStore.String = path;
        assemblyInfoItemsStore.Load(path);
    }

    public event EventHandler? CanExecuteChanged;
}
