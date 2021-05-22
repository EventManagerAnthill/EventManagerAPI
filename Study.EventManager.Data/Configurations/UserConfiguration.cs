using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.FirstName).IsRequired();
            builder.Property(t => t.LastName).IsRequired();
            builder.Property(t => t.Middlename);
            builder.Property(t => t.BirthDate);
            builder.Property(t => t.Phone);
            builder.Property(t => t.Email).IsRequired();
            builder.Property(t => t.Sex);
            builder.Property(t => t.Username);
            builder.Property(t => t.Password).IsRequired();
        }
    }
}
