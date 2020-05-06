using System;
using AccountManager.Domain.Models;

namespace AccountManager.API.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
