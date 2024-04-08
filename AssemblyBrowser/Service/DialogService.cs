using Microsoft.Win32;
namespace AssemblyBrowser.Service;

public class DialogService : IDialogService
{
    public string? OpenFileDialog(OpenFileDialogSettings settings)
    {
        var dialog = new OpenFileDialog
        {
            Title = settings.Title,
            Filter = settings.Filter,
            InitialDirectory = settings.InitialDirectory
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
