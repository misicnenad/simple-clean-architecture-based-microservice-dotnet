using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserManager.Infrastructure.Models
{
    public class UserDbo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<AccountDbo> Accounts { get; set; }
    }

    public class UserDboConfiguration : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(x => x.Accounts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.DateCreated)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
