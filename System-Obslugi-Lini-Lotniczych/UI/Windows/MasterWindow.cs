using System;
using System.Threading.Tasks;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

internal sealed class MasterWindow : IGWindow
{
    public bool PreserveContentOnTransferControl => false;
    public WindowUInterfaceManager Console { get; } = new ();
    public string Id => "__entry-point";
    public string Title => throw new NotImplementedException();
    public void Open()
    {
        UserInterfaceManager.Instance.OpenWindow("default");
    }

    public void Close()
    {
        Environment.Exit(0);
    }

    public Task Update()
    {
        throw new NotImplementedException();
    }

    public UserInterfaceElement[] UserInterfaceElements => throw new NotImplementedException();
    public InteractableUserInterfaceElement[] InteractableUserInterfaceElements => throw new NotImplementedException();
    public int CurrentElementIndex { get; set; }
    public void OnKeyPressed(ConsoleKeyInfo key)
    {
        throw new NotImplementedException();
    }

    public void Resume()
    {
        this.Close();
    }
}