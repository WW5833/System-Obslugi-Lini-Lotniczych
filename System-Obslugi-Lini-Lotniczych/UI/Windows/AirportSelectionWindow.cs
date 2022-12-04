using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class AirportSelectionWindow : FullScreenWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; } = null;

    public override string Id => "airport_selection";

    public override string Title => "airport_selection";
}
