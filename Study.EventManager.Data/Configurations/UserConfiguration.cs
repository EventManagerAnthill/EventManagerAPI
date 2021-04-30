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
            builder.Property(t => t.Surname);
            builder.Property(t => t.Name);
            builder.Property(t => t.Patron);
            builder.Property(t => t.Birth);
            builder.Property(t => t.Phone);
            builder.Property(t => t.Email);
            builder.Property(t => t.Sex);
        }
    }
}
