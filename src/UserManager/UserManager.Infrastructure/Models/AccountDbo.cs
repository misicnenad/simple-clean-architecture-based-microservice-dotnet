using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserManager.Infrastructure.Models
{
    public class AccountDbo
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public UserDbo User { get; set; }
    }

    public class AccountDboConfiguration : IEntityTypeConfiguration<AccountDbo>
    {
        public void Configure(EntityTypeBuilder<AccountDbo> builder)
        {
            builder.HasKey(x => x.AccountId);
            builder.Property(x => x.AccountId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.DateCreated)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
