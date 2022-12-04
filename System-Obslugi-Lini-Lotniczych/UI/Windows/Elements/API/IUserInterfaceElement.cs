namespace LotSystem.UI.Windows.Elements.API;

public interface IUserInterfaceElement
{
    WindowUInterfaceManager Console { get; }
    void Draw();
}