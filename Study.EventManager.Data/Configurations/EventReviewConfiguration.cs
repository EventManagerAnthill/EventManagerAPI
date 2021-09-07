using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
     
    public class EventReviewConfiguration : IEntityTypeConfiguration<EventReview>
    {
        public void Configure(EntityTypeBuilder<EventReview> builder)
        {
            builder.ToTable("EventReview");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.UserId).IsRequired();
            builder.Property(t => t.EventId).IsRequired();
            builder.Property(t => t.StarReview).IsRequired();
            builder.Property(t => t.TextReview);
          
            builder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
