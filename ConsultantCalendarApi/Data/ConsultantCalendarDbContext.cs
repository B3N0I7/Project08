using ConsultantCalendarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultantCalendarApi.Data
{
    public class ConsultantCalendarDbContext : DbContext
    {
        public ConsultantCalendarDbContext(DbContextOptions<ConsultantCalendarDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConsultantModel>();
            modelBuilder.Entity<ConsultantCalendarModel>();
        }

        public DbSet<ConsultantModel> Consultants { get; set; }
        public DbSet<ConsultantCalendarModel> ConsultantCalendars { get; set; }
    }
}
