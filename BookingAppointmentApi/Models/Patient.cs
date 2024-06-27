namespace BookingAppointmentApi.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string ContactNumber { get; set; }
    }
}
