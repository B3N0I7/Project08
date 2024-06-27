using BookingAppointmentApi.Models;
using BookingAppointmentApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppointmentApi.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AppointmentsController(IAppointmentRepository repository, ILogger<AppointmentsController> logger, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // Semaphore Slim
        private static readonly SemaphoreSlim _semaphoreSlim = new(
            initialCount: 10);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GetAllAppointments()
        {
            var appointments = await _repository.GetAllAppointmentsAsync();

            if (appointments == null)
            {
                _logger.LogError("No appointment found");

                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"List of Appointments retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(appointments);
        }

        [HttpGet("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Appointment>> GetOneAppointment(Guid appointmentId)
        {
            var appointment = await _repository.GetAppointmentAsync(appointmentId);

            if (appointment == null)
            {
                _logger.LogError($"Appointment {appointmentId} not found");

                return NotFound();
            }

            _logger.LogInformation($"Appointment {appointmentId} retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(appointment);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AppointmentModel>> CreateOneAppointment(AppointmentModel model)
        {
            if (model == null)
            {
                _logger.LogError("Invalid appointment data");

                return NotFound();
            }

            //Add semaphore et try/finally
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (string.IsNullOrEmpty(model.ConsultantName))
                {
                    _logger.LogError("Consultant name is required");

                    return BadRequest("Consultant name is required");
                }

                _logger.LogInformation($"Consultant name: {model.ConsultantName}");

                var consultantIdString = $"https://localhost:8885/Consultants/checkAvailability/{model.ConsultantName}";

                _logger.LogInformation($"Consultant ID string: {consultantIdString}");

                var _httpClient = _httpClientFactory.CreateClient();

                var consultantIdResponse = await _httpClient.GetAsync(consultantIdString);

                if (consultantIdResponse.IsSuccessStatusCode)
                {
                    var consultantId = await consultantIdResponse.Content.ReadFromJsonAsync<string>();

                    var checkAvailabilityString = $"https://localhost:8885/ConsultantCalendars/checkAvailability/{consultantId}?appointmentDateString={model.AppointmentDateString}";

                    var isAvailableResponse = await _httpClient.GetAsync(checkAvailabilityString);

                    if (isAvailableResponse.IsSuccessStatusCode)
                    {
                        var isAvailableStringContent = await isAvailableResponse.Content.ReadAsStringAsync();
                        var isAvailable = bool.Parse(isAvailableStringContent);

                        if (!isAvailable)
                        {
                            return BadRequest("Ko");
                        }
                    }
                }

                await _repository.CreateAppointmentAsync(model);

                _logger.LogInformation($"New appointment created successfully at {DateTime.UtcNow.ToLocalTime()}");

                return CreatedAtAction(nameof(GetOneAppointment), new { appointmentId = model.Id }, model);
            }
            finally
            {
                //semaphore.Release();
                _semaphoreSlim.Release();
            }
        }

        [HttpPut("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOneAppointment(Guid appointmentId, AppointmentModel model)
        {
            if (appointmentId != model?.Id)
            {
                _logger.LogError($"Appointment {appointmentId} mismatch");

                return NotFound();
            }

            await _repository.UpdateAppointmentAsync(model);

            _logger.LogInformation($"Appointment {appointmentId} updated successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok();
        }

        [HttpDelete("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOneAppointment(Guid appointmentId)
        {

            await _repository.DeleteAppointmentAsync(appointmentId);

            _logger.LogInformation($"Appointment {appointmentId} deleted successfully at {DateTime.UtcNow.ToLocalTime()}");

            return NoContent();
        }
    }
}
