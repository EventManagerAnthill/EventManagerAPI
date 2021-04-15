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
            builder.Property(t => t.CreateDt);
            builder.Property(t => t.HoldingDt);
            builder.Property(t => t.TypeId);
            builder.Property(t => t.UserId);
            builder.Property(t => t.Description);
        }
    }
}
