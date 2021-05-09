using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.Name);
            builder.Property(t => t.CreateDate);
            builder.Property(t => t.HoldingDate);
            builder.Property(t => t.Type);
            builder.Property(t => t.UserId);
            builder.Property(t => t.Description);

           // builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.User); 
        }
    }
}
