using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotSystem.Database.Models;

[Table("flights")]
public sealed class Flight
{
    [Key] public int Id { get; set; }

    public DateTime TakeOffTime { get; set; }
    public DateTime ArriveTime { get; set; }

    public int StartFromId { get; set; }
    public int ArriveAtId { get; set; }

    public FlightState State { get; set; }

    public byte SeatCount { get; set; }

    public Airport StartFrom { get; set; }
    public Airport ArriveAt { get; set; }
}