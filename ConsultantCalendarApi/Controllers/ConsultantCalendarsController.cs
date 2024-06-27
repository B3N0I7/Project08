using ConsultantCalendarApi.Models;
using ConsultantCalendarApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ConsultantCalendarApi.Controllers
{
    //[Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class ConsultantCalendarsController : ControllerBase
    {
        private readonly IConsultantCalendarRepository _repository;
        private readonly ILogger<ConsultantCalendarsController> _logger;

        public ConsultantCalendarsController(IConsultantCalendarRepository repository, ILogger<ConsultantCalendarsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ConsultantCalendarModel>>> GetAllConsultantCalendars()
        {
            var consultants = await _repository.GetAllConsultantCalendarsAsync();

            if (consultants == null)
            {
                _logger.LogError("No consultant found");

                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"List of ConsultantCalendars retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(consultants);
        }

        [HttpGet("{consultantCalendarId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultantCalendarModel>> GetOneConsultantCalendar(Guid consultantCalendarId)
        {
            var consultant = await _repository.GetConsultantCalendarAsync(consultantCalendarId);

            if (consultant == null)
            {
                _logger.LogError($"ConsultantCalendar {consultantCalendarId} not found");

                return NotFound();
            }

            _logger.LogInformation($"ConsultantCalendar {consultantCalendarId} retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(consultant);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConsultantCalendarModel>> CreateOneConsultantCalendar(ConsultantCalendarModel model)
        {
            if (model == null)
            {
                _logger.LogError("Invalid consultantCalendar data");

                return NotFound();
            }

            await _repository.CreateConsultantCalendarAsync(model);

            _logger.LogInformation($"New consultantCalendar created successfully at {DateTime.UtcNow.ToLocalTime()}");

            return CreatedAtAction(nameof(GetOneConsultantCalendar), new { consultantCalendarId = model.Id }, model);
        }

        [HttpPut("{consultantCalendarId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOneConsultantCalendar(Guid consultantCalendarId, ConsultantCalendarModel model)
        {
            if (consultantCalendarId != model?.Id)
            {
                _logger.LogError($"ConsultantCalendar {consultantCalendarId} mismatch");

                return NotFound();
            }

            await _repository.UpdateConsultantCalendarAsync(model);

            _logger.LogInformation($"ConsultantCalendar {consultantCalendarId} updated successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok();
        }

        [HttpDelete("{consultantCalendarId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOneConsultantCalendar(Guid consultantCalendarId)
        {
            await _repository.DeleteConsultantCalendarAsync(consultantCalendarId);

            _logger.LogInformation($"ConsultantCalendar {consultantCalendarId} deleted successfully at {DateTime.UtcNow.ToLocalTime()}");

            return NoContent();
        }

        [HttpGet("checkAvailability/{consultantId}")]
        public async Task<bool> GetAvailability(Guid consultantId, string appointmentDateString)
        {
            DateTime.TryParseExact(appointmentDateString, "dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out DateTime appointmentDate);

            var IsAvailable = await _repository.IsDateAvailable(consultantId, appointmentDate);

            if (IsAvailable)
            {
                //var appointment = await _repository.UpdateConsultantCalendarAsync(consultantId, appointmentDate, false);
                var appointment = await _repository.GetConsultantCalendarByConsultantIdAndDateAsync(consultantId, appointmentDate);
                if (appointment != null)
                {
                    appointment.IsAppointment = false;
                    await _repository.UpdateConsultantCalendarAsync(appointment);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
