using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
    public class EventUserLinkConfig : IEntityTypeConfiguration<EventUserLink>
    {
        public void Configure(EntityTypeBuilder<EventUserLink> builder)
        {
            builder.ToTable("EventUserLink");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.UserId);
            builder.Property(t => t.EventId);
            builder.Property(t => t.UserEventRole);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
