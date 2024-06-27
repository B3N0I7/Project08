using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosultantCalendarApi.Models
{
    public class ConsultantList
    {
        public List<ConsultantCalendar> ConsultantCalendars { get; set; }
        public List<Consultant> Consultants { get; set; }
        public int SelectedConsultantId { get; set; }
        public SelectList ConsultantsList { get; set; }
    }
}
