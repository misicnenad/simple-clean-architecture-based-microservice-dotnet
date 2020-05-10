using System;

namespace AccountManager.Domain.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   AccountId == account.AccountId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountId, UserId, AccountType, DateCreated);
        }
    }

    public enum AccountType
    {
        None = 0,
        Credit,
        Debit
    }
}
