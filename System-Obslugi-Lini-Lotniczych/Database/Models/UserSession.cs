using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotSystem.Database.Models
{
    [Table("users_sessions")]
    public sealed class UserSession
    {
        [Key] public Guid Id { get; set; }
        public int UserId { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime ExpireTime { get; set; }

        public bool IsExpiered { get; set; }

        public User User { get; set; }
    }
}