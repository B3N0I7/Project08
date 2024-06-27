using ConsultantCalendarApi.Models;

namespace ConsultantApi.Repositories
{
    public interface IConsultantRepository
    {
        Task<IEnumerable<ConsultantModel>> GetAllConsultantsAsync();
        Task<ConsultantModel> GetConsultantAsync(Guid consultantId);
        Task<int> CreateConsultantAsync(ConsultantModel model);
        Task UpdateConsultantAsync(ConsultantModel model);
        Task<int> DeleteConsultantAsync(Guid consultantId);
        Task<Guid> GetConsultantIdByNameAsync(string consultantName);
    }
}
