using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookingAppointmentApi.Models
{
    public class AppointmentModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Le champ nom du patient est vide")]
        [DisplayName("Nom du patient")]
        public string PatientName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le champ nom du consultant est vide")]
        [DisplayName("Nom du consultant")]
        public string ConsultantName { get; set; } = string.Empty;
        //[Required(ErrorMessage = "Le champ spécialité du consultant est vide")]
        [DisplayName("Spécialité du consultant")]
        public string ConsultantSpecialty { get; set; } = string.Empty;
        [Required(ErrorMessage = "la date ne doit pas être vide")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string AppointmentDateString => AppointmentDate.ToString("dd/MM/yyyy");
    }
}
