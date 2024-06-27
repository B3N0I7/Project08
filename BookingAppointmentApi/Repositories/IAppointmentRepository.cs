using BookingAppointmentApi.Models;

namespace BookingAppointmentApi.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync();
        Task<AppointmentModel> GetAppointmentAsync(Guid appointmentId);
        Task<int> CreateAppointmentAsync(AppointmentModel model);
        Task UpdateAppointmentAsync(AppointmentModel model);
        Task<int> DeleteAppointmentAsync(Guid appointmentId);
    }
}
