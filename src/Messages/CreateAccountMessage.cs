using System;

namespace Shared.Messages
{
    public class CreateAccountMessage : IMessage
    {
        public CreateAccountMessage(Guid correlationId, int userId, AccountType accountType)
        {
            CorrelationId = correlationId;
            UserId = userId;
            AccountType = accountType;
        }

        public Guid CorrelationId { get; }
        public int UserId { get; }
        public AccountType AccountType { get; }
    }

    public enum AccountType
    {
        None = 0,
        Credit,
        Debit
    }
}
