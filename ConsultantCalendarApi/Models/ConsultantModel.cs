namespace ConsultantCalendarApi.Models
{
    public class ConsultantModel
    {
        public Guid Id { get; set; }
        public string ConsultantName { get; set; } = string.Empty;
        public string ConsultantSpecialty { get; set; } = string.Empty;

        //public ICollection<ConsultantCalendarModel> ConsultantCalendarModels { get; set; }
    }
}
