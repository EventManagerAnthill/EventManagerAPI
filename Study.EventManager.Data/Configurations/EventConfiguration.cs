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
            builder.Property(t => t.BeginHoldingDate);
            builder.Property(t => t.HoldingDate);
            builder.Property(t => t.EventTimeZone);
            builder.Property(t => t.Type);
            builder.Property(t => t.UserId);
            builder.Property(t => t.CompanyId);
            builder.Property(t => t.Status);
            builder.Property(t => t.Description);
            builder.Property(t => t.Del);
            builder.Property(t => t.OriginalFileName);
            builder.Property(t => t.ServerFileName);
            builder.Property(t => t.FotoUrl);

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
