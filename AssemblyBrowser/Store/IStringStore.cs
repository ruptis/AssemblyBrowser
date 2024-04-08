namespace AssemblyBrowser.Store;

public interface IStringStore
{
    public string String { get; }
    public event Action? StringChanged;
}