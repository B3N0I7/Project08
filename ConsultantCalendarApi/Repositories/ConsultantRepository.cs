using ConsultantCalendarApi.Data;
using ConsultantCalendarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultantApi.Repositories
{
    public class ConsultantRepository : IConsultantRepository
    {
        private readonly ConsultantCalendarDbContext _context;

        public ConsultantRepository(ConsultantCalendarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConsultantModel>> GetAllConsultantsAsync()
        {
            return await _context.Consultants.ToListAsync();
        }

        public async Task<ConsultantModel> GetConsultantAsync(Guid consultantId)
        {
            return await _context.Consultants.FindAsync(consultantId) ?? throw new InvalidOperationException();
        }

        public async Task<int> CreateConsultantAsync(ConsultantModel model)
        {
            _context.Consultants.Add(model);

            return await _context.SaveChangesAsync();
        }

        public async Task UpdateConsultantAsync(ConsultantModel model)
        {
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteConsultantAsync(Guid consultantId)
        {
            var consultant = await _context.Consultants.FindAsync(consultantId);
            if (consultant != null)
            {
                _context.Consultants.Remove(consultant);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<Guid> GetConsultantIdByNameAsync(string consultantName)
        {
            var consultants = await _context.Consultants.ToListAsync();

            var consultant = consultants.FirstOrDefault(c => c.ConsultantName == consultantName);

            if (consultant != null)
            {
                return consultant.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
