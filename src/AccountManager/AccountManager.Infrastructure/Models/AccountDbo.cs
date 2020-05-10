using System;
using AccountManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManager.Infrastructure.Models
{
    public class AccountDbo
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AccountDbo dbo &&
                   AccountId == dbo.AccountId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountId, UserId, AccountType, DateCreated);
        }
    }

    public class AccountDboConfiguration : IEntityTypeConfiguration<AccountDbo>
    {
        public void Configure(EntityTypeBuilder<AccountDbo> builder)
        {
            builder.HasKey(x => x.AccountId);
            builder.Property(x => x.AccountId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.DateCreated)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
