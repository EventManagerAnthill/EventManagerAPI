using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Configurations
{
    public class CompanyUserLinkConfig : IEntityTypeConfiguration<CompanyUserLink>
    {
        public void Configure(EntityTypeBuilder<CompanyUserLink> builder)
        {
            builder.ToTable("CompanyUserLink");
            builder.HasKey(o => o.Id);            
            builder.Property(t => t.UserId);
            builder.Property(t => t.CompanyId);
            builder.Property(t => t.UserCompanyRole);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
