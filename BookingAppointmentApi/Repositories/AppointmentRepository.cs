using BookingAppointmentApi.Data;
using BookingAppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAppointmentApi.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BookingAppointmentDbContext _context;
        private readonly IHttpClientFactory _httpClient;

        public AppointmentRepository(BookingAppointmentDbContext context, IHttpClientFactory httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<AppointmentModel> GetAppointmentAsync(Guid appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId) ?? throw new InvalidOperationException();
        }

        public async Task<int> CreateAppointmentAsync(AppointmentModel model)
        {
            var consultantUrl = $"https://localhost:8885/Consultants/GetGuidConsultant" +
                $"?consultantName={model.ConsultantName}";

            var httpClient = _httpClient.CreateClient();

            var consultantId = await httpClient.GetAsync(consultantUrl);

            var availableUrl = $"https://localhost:8885/ConsultantCalendars/GetAvailability" +
                $"?consultantId={consultantId}";

            var isAvailable = await httpClient.GetAsync(availableUrl);

            if (isAvailable.IsSuccessStatusCode)
            {
                var isAvailableResponse = await isAvailable.Content.ReadFromJsonAsync<bool>();

                if (!isAvailableResponse)
                {
                    return 0;
                }
            }

            _context.Appointments.Add(model);

            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(AppointmentModel model)
        {
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
