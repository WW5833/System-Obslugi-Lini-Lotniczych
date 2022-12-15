using LotSystem.Database.Models;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using System;
using System.Collections.Generic;

namespace LotSystem.UI.Windows;

public class SeatSelectionWindow : ModalWindow, IModalSelectWindow<string>
{
    public override int Width => 50;

    public override int Height => 30;

    public override UserInterfaceElement[] UserInterfaceElements => _userInterfaceElements;
    private UserInterfaceElement[] _userInterfaceElements;

    public override string Id => "seat-select-window";

    public override string Title => "Select seat";

    public string Value { get; private set; }

    public override void Open()
    {
        var flight = BuyTicketWindow.Flight;
        var elements = new List<UserInterfaceElement>();

        if (flight.SeatCount % 9 == 0)
            WriteWith3Rows(flight, elements);
        else if (flight.SeatCount % 6 == 0)
            WriteWith2Rows(flight, elements);
        else
            throw new Exception("Invalid seat count");

        _userInterfaceElements = elements.ToArray();

        base.Open();
    }

    private void WriteWith2Rows(Flight flight, List<UserInterfaceElement> list)
    {
        int rows = flight.SeatCount / 6;
        var initialLablel = new Label(this, "∙–––––––∙");
        list.Add(initialLablel);
        for (int i = 0; i < rows; i++)
        {
            list.Add(new Label(this, "|       |"));
        }

        list.Add(new Label(this, "∙–––––––∙"));

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                list.Add(new Button(this, "X", () => { }, StartLeft + 3 + j + (j > 2 ? 1 : 0), StartTop + i + 2));
            }
        }
    }

    private void WriteWith3Rows(Flight flight, List<UserInterfaceElement> list)
    {
        int rows = flight.SeatCount / 9;
        list.Add(new Label(this, "∙–––––––––––∙"));
        for (int i = 0; i < rows; i++)
        {
            var label = new Label(this, "|           |");
            list.Add(label);

            for (int j = 0; j < 9; j++)
            {
                list.Add(new Button(this, "X", () => { }, label.XStartPos + 1 + j, label.YPos));
            }
        }

        list.Add(new Label(this, "∙–––––––––––∙"));
    }
}