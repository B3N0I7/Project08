using HspFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace HspFrontEnd.Controllers
{
    public class CalendarController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CalendarController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public CalendarController(IHttpClientFactory httpClient, ILogger<CalendarController> logger, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:8882");
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index(string? consultantFilter = null, string? monthFilter = null, string? yearFilter = null)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("/gateway/consultantcalendars");

            if (request.IsSuccessStatusCode)
            {
                var consultantCalendars = await request.Content.ReadFromJsonAsync<List<ConsultantCalendarDto>>();

                if (!string.IsNullOrEmpty(consultantFilter))
                {
                    consultantCalendars = consultantCalendars.Where(c => c.ConsultantName.Contains(consultantFilter)).ToList();
                }

                if (!string.IsNullOrEmpty(monthFilter))
                {
                    int.TryParse(monthFilter, out var monthNumber);
                    consultantCalendars = consultantCalendars.Where(m => m.AppointmentDate.Month == monthNumber).ToList();
                }

                if (!string.IsNullOrEmpty(yearFilter))
                {
                    int.TryParse(yearFilter, out var yearNumber);
                    consultantCalendars = consultantCalendars.Where(y => y.AppointmentDate.Year == yearNumber).ToList();
                }

                consultantCalendars = consultantCalendars.Where(cc => cc.IsAppointment).ToList().OrderBy(cc => cc.AppointmentDate).ToList();

                _logger.LogInformation($"List of calendars retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

                return View(consultantCalendars);
            }
            else
            {
                _logger.LogError("Error retrieving calendars");

                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
