using BookingAppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAppointmentApi.Data
{
    public class BookingAppointmentDbContext : DbContext
    {
        public BookingAppointmentDbContext(DbContextOptions<BookingAppointmentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppointmentModel> Appointments { get; set; }
    }
}
