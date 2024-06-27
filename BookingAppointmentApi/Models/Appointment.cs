namespace BookingAppointmentApi.Models
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}
