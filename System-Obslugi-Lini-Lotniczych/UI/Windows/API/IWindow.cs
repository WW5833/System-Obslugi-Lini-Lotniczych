namespace LotSystem.UI.Windows.API;

public interface IWindow
{
    WindowUInterfaceManager Console { get; }
    string Id { get; }
    string Title { get; }
    void Open();
    void Close();
}