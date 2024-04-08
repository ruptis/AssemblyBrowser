namespace AssemblyBrowser.Service;

public interface IDialogService
{
    public string? OpenFileDialog(OpenFileDialogSettings settings);
}
public record OpenFileDialogSettings(string Title, string Filter, string InitialDirectory);
