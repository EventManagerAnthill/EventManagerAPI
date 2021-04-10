using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Configurations;
using System;

namespace Study.EventManager.Data
{
    public class EventManagerDBContext : DBContext
    {
        private readonly string _connectionStr;
        public EventManagerDBContext(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionStr);
        }

        protected override void OnModeCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        }
    }
}
