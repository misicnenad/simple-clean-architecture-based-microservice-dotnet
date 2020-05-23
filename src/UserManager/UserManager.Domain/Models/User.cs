using System;
using System.Collections.Generic;

namespace UserManager.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<Account> Accounts { get; set; } = new List<Account>();
    }
}
