using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
     
    public class CompanySubConfiguration : IEntityTypeConfiguration<CompanySubscription>
    {
        public void Configure(EntityTypeBuilder<CompanySubscription> builder)
        {
            builder.ToTable("CompanySubscription");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.UserId).IsRequired();
            builder.Property(t => t.CompanyId).IsRequired();
            builder.Property(t => t.SubEndDt).IsRequired();
            builder.Property(t => t.UseTrialVersion).IsRequired();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
