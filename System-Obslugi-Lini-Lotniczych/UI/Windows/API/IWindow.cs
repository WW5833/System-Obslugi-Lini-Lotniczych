namespace LotSystem.UI.Windows.API;

public interface IWindow
{
    bool PreserveContentOnTransferControl { get; }
    WindowUInterfaceManager Console { get; }
    string Id { get; }
    string Title { get; }
    void Open();
    void Close();
}