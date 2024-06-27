using ConsultantApi.Repositories;
using ConsultantCalendarApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantApi.Controllers
{
    //[Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class ConsultantsController : ControllerBase
    {
        private readonly IConsultantRepository _repository;
        private readonly ILogger<ConsultantsController> _logger;

        public ConsultantsController(IConsultantRepository repository, ILogger<ConsultantsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ConsultantModel>>> GetAllConsultants()
        {
            var consultants = await _repository.GetAllConsultantsAsync();

            if (consultants == null)
            {
                _logger.LogError("No consultant found");

                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"List of Consultants retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(consultants);
        }

        [HttpGet("{consultantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultantModel>> GetOneConsultant(Guid consultantId)
        {
            var consultant = await _repository.GetConsultantAsync(consultantId);

            if (consultant == null)
            {
                _logger.LogError($"Consultant {consultantId} not found");

                return NotFound();
            }

            _logger.LogInformation($"Consultant {consultantId} retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(consultant);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConsultantModel>> CreateOneConsultant(ConsultantModel model)
        {
            if (model == null)
            {
                _logger.LogError("Invalid consultant data");

                return NotFound();
            }

            await _repository.CreateConsultantAsync(model);

            _logger.LogInformation($"New consultant created successfully at {DateTime.UtcNow.ToLocalTime()}");

            return CreatedAtAction(nameof(GetOneConsultant), new { consultantId = model.Id }, model);
        }

        [HttpPut("{consultantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOneConsultant(Guid consultantId, ConsultantModel model)
        {
            if (consultantId != model?.Id)
            {
                _logger.LogError($"Consultant {consultantId} mismatch");

                return NotFound();
            }

            await _repository.UpdateConsultantAsync(model);

            _logger.LogInformation($"Consultant {consultantId} updated successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok();
        }

        [HttpDelete("{consultantId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOneConsultant(Guid consultantId)
        {
            await _repository.DeleteConsultantAsync(consultantId);

            _logger.LogInformation($"Consultant {consultantId} deleted successfully at {DateTime.UtcNow.ToLocalTime()}");

            return NoContent();
        }

        [HttpGet("checkAvailability/{consultantName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetGuidConsultant(string consultantName)
        {
            _logger.LogInformation($"Consultant name: {consultantName}");

            var consultant = await _repository.GetConsultantIdByNameAsync(consultantName);

            if (consultant == null)
            {
                _logger.LogError($"Consultant {consultantName} not found");

                return NotFound();
            }

            _logger.LogInformation($"Consultant {consultantName} retrieved successfully at {DateTime.UtcNow.ToLocalTime()}");

            return Ok(consultant);
        }
    }
}
