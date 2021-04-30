using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Configurations;
using System;

namespace Study.EventManager.Data
{
    public class EventManagerDbContext : DbContext
    {     
        public EventManagerDbContext(DbContextOptions options)
            :base(options)
        {
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
