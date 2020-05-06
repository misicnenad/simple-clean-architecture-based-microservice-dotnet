using System;

namespace AccountManager.Domain.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public enum AccountType
    {
        None = 0,
        Credit,
        Debit
    }
}
