using System;

namespace UserManager.Domain.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public User User { get; set; }
    }
}
