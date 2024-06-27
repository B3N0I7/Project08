using ConsultantCalendarApi.Data;
using ConsultantCalendarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultantCalendarApi.Repositories
{
    public class ConsultantCalendarRepository : IConsultantCalendarRepository
    {
        private readonly ConsultantCalendarDbContext _context;

        public ConsultantCalendarRepository(ConsultantCalendarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConsultantCalendarModel>> GetAllConsultantCalendarsAsync()
        {
            return await _context.ConsultantCalendars.ToListAsync();
        }

        public async Task<ConsultantCalendarModel> GetConsultantCalendarAsync(Guid consultantCalendarId)
        {
            return await _context.ConsultantCalendars.FindAsync(consultantCalendarId) ?? throw new InvalidOperationException();
        }

        public async Task<int> CreateConsultantCalendarAsync(ConsultantCalendarModel model)
        {
            _context.ConsultantCalendars.Add(model);

            return await _context.SaveChangesAsync();
        }

        public async Task UpdateConsultantCalendarAsync(ConsultantCalendarModel model)
        {
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteConsultantCalendarAsync(Guid consultantCalendarId)
        {
            var consultantCalendar = await _context.ConsultantCalendars.FindAsync(consultantCalendarId);
            if (consultantCalendar != null)
            {
                _context.ConsultantCalendars.Remove(consultantCalendar);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDateAvailable(Guid consultantCalendarId, DateTime appointmentDate)
        {
            var appointments = await _context.ConsultantCalendars.ToListAsync();

            var appointment = appointments.FirstOrDefault(a => a.ConsultantId == consultantCalendarId && a.AppointmentDate == appointmentDate);

            if (appointment != null && appointment.IsAppointment)
            {
                return appointment.IsAppointment;
            }
            else
            {
                return false;
            }
        }

        public async Task<ConsultantCalendarModel> GetConsultantCalendarByConsultantIdAndDateAsync(Guid consultantCalendarId, DateTime appointmentDate)
        {
            return await _context.ConsultantCalendars
                .FirstOrDefaultAsync(c => c.ConsultantId == consultantCalendarId && c.AppointmentDate == appointmentDate)
                ?? throw new InvalidOperationException();
        }
    }
}
