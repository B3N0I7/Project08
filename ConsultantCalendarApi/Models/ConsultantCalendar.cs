namespace CosultantCalendarApi.Models
{
    public class ConsultantCalendar
    {
        public int Id { get; set; }
        public string ConsultantName { get; set; }

        public List<DateTime> AvailableDates { get; set; }
    }
}
