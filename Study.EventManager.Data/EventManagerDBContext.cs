using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Configurations;
using Study.EventManager.Model;
using System;

namespace Study.EventManager.Data
{
    public class EventManagerDbContext : DbContext
    {     
        public EventManagerDbContext(DbContextOptions options) :base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<EventUserLink> EventUsers { get; set; }

        public DbSet<CompanyUserLink> CompanyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyUserLinkConfig());
            modelBuilder.ApplyConfiguration(new EventUserLinkConfig());

/*            modelBuilder.Entity<User>()
                .HasMany(c => c.Companies)
                .WithMany(u => u.Users);
                
            modelBuilder.Entity<User>()
                .HasMany(c => c.Events)
                .WithMany(u => u.Users);  */               
        }
    }
}
