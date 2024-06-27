using HspFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HspFrontEnd.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookingController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingController(IHttpClientFactory httpClient, ILogger<BookingController> logger, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:8882");
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? consultantFilter = null, string? monthFilter = null, string? yearFilter = null)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("/gateway/appointments");

            if (request.IsSuccessStatusCode)
            {
                var appointments = await request.Content.ReadFromJsonAsync<List<AppointmentDto>>();

                if (!string.IsNullOrEmpty(consultantFilter))
                {
                    appointments = appointments.Where(a => a.ConsultantName.Contains(consultantFilter)).ToList();
                }

                if (!string.IsNullOrEmpty(monthFilter))
                {
                    int.TryParse(monthFilter, out int monthNumber);
                    appointments = appointments.Where(a => a.AppointmentDate.Month == monthNumber).ToList();
                }

                if (!string.IsNullOrEmpty(yearFilter))
                {
                    int.TryParse(yearFilter, out int yearNumber);
                    appointments = appointments.Where(a => a.AppointmentDate.Year == yearNumber).ToList();
                }

                _logger.LogInformation($"List of appointments retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

                return View(appointments.OrderBy(a => a.AppointmentDate));
            }
            else
            {
                _logger.LogError("Error retrieving appointments");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentDto appointment)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(appointment), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/gateway/appointments", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Appointment added successfully");

                TempData["SuccessMessage"] = "Rendez-vous confirmé.";

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong");

                TempData["ErrorMessage"] = "Une erreur s'est produite lors de la création du rendez-vous.";

                return View();
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/gateway/appointments/{id}");

            if (response.IsSuccessStatusCode)
            {
                var appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return View(appointment);
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Unable to retrieve patient details");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReallyDelete(Guid id)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"/gateway/appointments/{id}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Appointment with ID {id} deleted successfully");

                TempData["SuccessMessage"] = "Rendez-vous supprimé.";

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong while deleting appointment with ID {id}");
                return View("Error");
            }
        }
    }
}
