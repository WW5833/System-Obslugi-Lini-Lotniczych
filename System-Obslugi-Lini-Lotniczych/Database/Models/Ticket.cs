using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotSystem.Database.Models
{
    [Table("tickets")]
    public sealed class Ticket
    {
        [Key] public int Id { get; set; }

        public int UserId { get; set; }
        public int FlightId { get; set; }

        public TicketState State { get; set; }
        public TicketFlags Flags { get; set; }

        [MinLength(2)][MaxLength(3)] public string Seat { get; set; }

        public DateTime? BoughtTime { get; set; }

        public User User { get; set; }
        public Flight Flight { get; set; }
    }
}