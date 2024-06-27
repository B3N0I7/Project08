using System.ComponentModel.DataAnnotations;

namespace HspFrontEnd.Models
{
    public class ConsultantCalendarDto
    {
        public Guid Id { get; set; }
        public Guid ConsultantId { get; set; }
        public string ConsultantName { get; set; } = string.Empty;
        public string ConsultantSpecialty { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string AppointmentDateString => AppointmentDate.ToString("dd/MM/yyyy");
        public bool IsAppointment { get; set; }
    }
}
