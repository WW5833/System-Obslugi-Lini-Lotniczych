using System;

namespace LotSystem.UI.Windows.Elements.API;

public interface IInteractable
{
    void OnSelected();
    void OnUnselected();
    void OnKeyPressed(ConsoleKeyInfo key);

    void OnWindowResumed();
}