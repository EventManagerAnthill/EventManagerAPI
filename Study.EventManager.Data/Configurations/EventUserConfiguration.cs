using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
    public class EventUserConfiguration : IEntityTypeConfiguration<EventUser>
    {
        public void Configure(EntityTypeBuilder<EventUser> builder)
        {
            builder.ToTable("EventUser");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.EventId).IsRequired();
            builder.Property(t => t.UserId).IsRequired();

            builder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);            
        }
    }
}
