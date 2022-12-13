using System.Linq;
using LotSystem.Database.Models;
using LotSystem.Services.Airport;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class AirportSelectionWindow : ModalWindow, IModalSelectWindow<Airport>
{
    private readonly IAirportService _airportService;
    public Airport Value { get; private set; }
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
            UserInterfaceElements[i] = new Button(this, $"{airport.Name} ({airport.ShortName})", () => OnAirportSelected(airport));
        }

        UserInterfaceElements[^2] = new Separator(this);
        UserInterfaceElements[^1] = new CloseButton(this);
    }

    private void OnAirportSelected(Airport airort)
    {
        Value = airort;
        CloseThisWindow();
    }
}
