namespace AssemblyBrowser.Store;

public sealed class StringStore : IStringStore
{
    private string _string = string.Empty;

    public string String
    {
        get => _string;
        set
        {
            _string = value;
            StringChanged?.Invoke();
        }
    }
    
    public event Action? StringChanged;
}
