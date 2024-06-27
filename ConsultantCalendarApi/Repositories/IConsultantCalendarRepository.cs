using ConsultantCalendarApi.Models;

namespace ConsultantCalendarApi.Repositories
{
    public interface IConsultantCalendarRepository
    {
        Task<IEnumerable<ConsultantCalendarModel>> GetAllConsultantCalendarsAsync();
        Task<ConsultantCalendarModel> GetConsultantCalendarAsync(Guid consultantCalendarId);
        Task<int> CreateConsultantCalendarAsync(ConsultantCalendarModel model);
        Task UpdateConsultantCalendarAsync(ConsultantCalendarModel model);
        Task<int> DeleteConsultantCalendarAsync(Guid consultantCalendarId);
        Task<bool> IsDateAvailable(Guid consultantId, DateTime appointmentDate);
        Task<ConsultantCalendarModel> GetConsultantCalendarByConsultantIdAndDateAsync(Guid consultantCalendarId, DateTime appointmentDate);
    }
}
