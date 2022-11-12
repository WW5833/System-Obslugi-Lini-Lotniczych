﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotSystem.Database.Models
{
    [Table("users")]
    public sealed class User
    {
        [Key] public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; } // Todo: Hashed

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}