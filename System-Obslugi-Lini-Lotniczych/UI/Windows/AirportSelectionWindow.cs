using System.Linq;
using LotSystem.Services.Airport;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class AirportSelectionWindow : ModalWindow
{
    private readonly IAirportService _airportService;
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "airport_selection";

    public override string Title => "airport_selection";

    public override int Width => 30;

    public override int Height => UserInterfaceElements.Length + 5;

    public AirportSelectionWindow(IAirportService airportService)
    {
        _airportService = airportService;
        var airports = airportService.GetAirports().WaitAndReturn().ToArray();
        UserInterfaceElements = new UserInterfaceElement[2 + airports.Length];
        for (int i = 0; i < airports.Length; i++)
        {
            var airport = airports[i];
            UserInterfaceElements[i] = new Button(this, airport.Name, () => OnAirportSelected(airport.Name));
        }

        UserInterfaceElements[^2] = new Separator(this);
        UserInterfaceElements[^1] = new CloseButton(this);
    }

    private void OnAirportSelected(string name)
    {
        AlertWindow.Show("Airport Selected", $"Selected: {name}");
    }
}
