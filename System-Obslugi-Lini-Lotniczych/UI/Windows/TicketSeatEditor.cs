using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;
using LotSystem.UI.Windows.Elements.InputFields;

namespace LotSystem.UI.Windows;

public sealed class TicketSeatEditor : ModalWindow, IModalSelectWindow<string>
{
    public override string Id { get; } = string.Empty;
    public override string Title { get; } = "Edit seat";
    public override UserInterfaceElement[] UserInterfaceElements { get; }
    public override int Width { get; } = 30;
    public override int Height { get; } = 3;

    private readonly SeatInputField _seatInputField;

    public string Value => _seatInputField.Value;
    public TicketSeatEditor(string value, int rows)
    {
        _seatInputField = new SeatInputField(this, "Seat: ", rows, value);

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _seatInputField
        };
    }
}