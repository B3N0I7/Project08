namespace BookingAppointmentApi.Models
{
    public class Consultant
    {
        public int ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string ConsultantSpeciality { get; set; }
        public int FacilityId { get; set; }
    }
}
