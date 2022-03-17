using MvpVendingMachineApp.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MvpVendingMachineApp.Persistance.Configuration.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasIndex(e => e.UserName)
                .IsUnique();

            builder
                .Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Role)
                .HasConversion(
            v => v.ToString(),
            v => (Role)Enum.Parse(typeof(Role), v));
        }
    }
}
