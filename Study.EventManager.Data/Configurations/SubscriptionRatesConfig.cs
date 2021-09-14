using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
     
    public class SubscriptionRatesConfig : IEntityTypeConfiguration<SubscriptionRates>
    {
        public void Configure(EntityTypeBuilder<SubscriptionRates> builder)
        {
            builder.ToTable("SubscriptionRates");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.ValidityDays).IsRequired();
            builder.Property(t => t.Price).IsRequired();            
            builder.Property(t => t.isFree).IsRequired();
            builder.Property(t => t.Description).IsRequired();
        }
    }
}
