using ConsultantCalendarApi.Controllers;
using ConsultantCalendarApi.Models;
using ConsultantCalendarApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConsultantCalendarApiTests
{
    public class ConsultantCalendarsControllerTests
    {
        private readonly Mock<IConsultantCalendarRepository> _mockRepository;
        private readonly Mock<ILogger<ConsultantCalendarsController>> _mockLogger;
        private readonly ConsultantCalendarsController _controller;

        public ConsultantCalendarsControllerTests()
        {
            _mockRepository = new Mock<IConsultantCalendarRepository>();
            _mockLogger = new Mock<ILogger<ConsultantCalendarsController>>();
            _controller = new ConsultantCalendarsController(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllConsultantCalendars_ReturnsOkObjectResult_WhenConsultantsExist()
        {
            // Arrange
            var consultants = new List<ConsultantCalendarModel>
            {
                new ConsultantCalendarModel { Id = Guid.NewGuid(), ConsultantId = Guid.NewGuid(), IsAppointment = true },
                new ConsultantCalendarModel { Id = Guid.NewGuid(), ConsultantId = Guid.NewGuid(), IsAppointment = false }
            };

            _mockRepository.Setup(repo => repo.GetAllConsultantCalendarsAsync()).ReturnsAsync(consultants);

            // Act
            var result = await _controller.GetAllConsultantCalendars();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(consultants);
        }

        [Fact]
        public async Task GetAllConsultantCalendars_ReturnsNotFound_WhenNoConsultantsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllConsultantCalendarsAsync()).ReturnsAsync((IEnumerable<ConsultantCalendarModel>)null);

            // Act
            var result = await _controller.GetAllConsultantCalendars();

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().BeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetOneConsultantCalendar_ReturnsOkObjectResult_WhenConsultantExists()
        {
            // Arrange
            var consultantId = Guid.NewGuid();
            var expectedConsultant = new ConsultantCalendarModel { Id = consultantId, ConsultantId = Guid.NewGuid(), IsAppointment = true };

            _mockRepository.Setup(repo => repo.GetConsultantCalendarAsync(consultantId)).ReturnsAsync(expectedConsultant);

            // Act
            var result = await _controller.GetOneConsultantCalendar(consultantId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(expectedConsultant);
        }

        [Fact]
        public async Task GetOneConsultantCalendar_ReturnsNotFound_WhenConsultantDoesNotExist()
        {
            // Arrange
            var consultantId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetConsultantCalendarAsync(consultantId)).ReturnsAsync((ConsultantCalendarModel)null);

            // Act
            var result = await _controller.GetOneConsultantCalendar(consultantId);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateOneConsultantCalendar_ReturnsBadRequest_WhenConsultantCalendarIsNull()
        {
            // Arrange
            ConsultantCalendarModel model = null;

            // Act
            var result = await _controller.CreateOneConsultantCalendar(model);

            // Assert
            var badRequestResult = result.Result as BadRequestResult;
            badRequestResult.Should().BeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateOneConsultantCalendar_ReturnsOkResult_WhenConsultantCalendarExists()
        {
            // Arrange
            var consultantCalendarId = Guid.NewGuid();
            var model = new ConsultantCalendarModel
            {
                Id = consultantCalendarId,
                ConsultantId = Guid.NewGuid(),
                IsAppointment = true
            };

            _mockRepository.Setup(repo => repo.UpdateConsultantCalendarAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOneConsultantCalendar(consultantCalendarId, model);

            // Assert
            var okResult = result as OkResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateOneConsultantCalendar_ReturnsNotFound_WhenConsultantCalendarDoesNotExist()
        {
            // Arrange
            var consultantCalendarId = Guid.NewGuid();
            var model = new ConsultantCalendarModel
            {
                Id = consultantCalendarId,
                ConsultantId = Guid.NewGuid(),
                IsAppointment = true
            };

            _mockRepository.Setup(repo => repo.UpdateConsultantCalendarAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOneConsultantCalendar(Guid.Empty, model);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }
    }
}