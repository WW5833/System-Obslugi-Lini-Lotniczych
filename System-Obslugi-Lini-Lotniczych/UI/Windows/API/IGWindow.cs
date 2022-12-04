using System;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.API;

public interface IGWindow : IWindow
{
    UserInterfaceElement[] UserInterfaceElements { get; }
    InteractableUserInterfaceElement[] InteractableUserInterfaceElements { get; }
    int CurrentElementIndex { get; set; }

    void OnKeyPressed(ConsoleKeyInfo key);

    void Resume();
}