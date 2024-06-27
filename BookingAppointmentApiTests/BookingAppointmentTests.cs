using BookingAppointmentApi.Controllers;
using BookingAppointmentApi.Models;
using BookingAppointmentApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingAppointmentApiTests
{
    public class AppointmentsControllerTests
    {
        private readonly Mock<IAppointmentRepository> _mockRepository;
        private readonly Mock<ILogger<AppointmentsController>> _mockLogger;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly AppointmentsController _controller;

        public AppointmentsControllerTests()
        {
            _mockRepository = new Mock<IAppointmentRepository>();
            _mockLogger = new Mock<ILogger<AppointmentsController>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _controller = new AppointmentsController(_mockRepository.Object, _mockLogger.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetAllAppointments_ReturnsOkObjectResult_WhenAppointmentsExist()
        {
            // Arrange
            var appointments = new List<AppointmentModel>
            {
                new AppointmentModel { Id = Guid.NewGuid(), ConsultantName = "Consultant A" },
                new AppointmentModel { Id = Guid.NewGuid(), ConsultantName = "Consultant B" }
            };

            _mockRepository.Setup(repo => repo.GetAllAppointmentsAsync()).ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAllAppointments();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(appointments);
        }

        [Fact]
        public async Task GetAllAppointments_ReturnsNotFound_WhenNoAppointmentsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAppointmentsAsync()).ReturnsAsync((IEnumerable<AppointmentModel>)null);

            // Act
            var result = await _controller.GetAllAppointments();

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().BeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetOneAppointment_ReturnsOkObjectResult_WhenAppointmentExists()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var expectedAppointment = new AppointmentModel { Id = appointmentId, ConsultantName = "Consultant A" };

            _mockRepository.Setup(repo => repo.GetAppointmentAsync(appointmentId)).ReturnsAsync(expectedAppointment);

            // Act
            var result = await _controller.GetOneAppointment(appointmentId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(expectedAppointment);
        }

        [Fact]
        public async Task GetOneAppointment_ReturnsNotFound_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetAppointmentAsync(appointmentId)).ReturnsAsync((AppointmentModel)null);

            // Act
            var result = await _controller.GetOneAppointment(appointmentId);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateOneAppointment_ReturnsBadRequest_WhenAppointmentIsNull()
        {
            // Arrange
            AppointmentModel model = null;

            // Act
            var result = await _controller.CreateOneAppointment(model);

            // Assert
            var badRequestResult = result.Result as BadRequestResult;
            badRequestResult.Should().BeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task CreateOneAppointment_ReturnsBadRequest_WhenConsultantNameIsNullOrEmpty()
        {
            // Arrange
            var model = new AppointmentModel
            {
                Id = Guid.NewGuid(),
                ConsultantName = string.Empty,
                AppointmentDate = new DateTime(2024 - 01 - 01)
            };

            // Act
            var result = await _controller.CreateOneAppointment(model);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.StatusCode.Should().Be(400);
            badRequestResult?.Value.Should().Be("Consultant name is required");
        }

        [Fact]
        public async Task UpdateOneAppointment_ReturnsOkResult_WhenAppointmentExists()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var model = new AppointmentModel
            {
                Id = appointmentId,
                ConsultantName = "Consultant A",
                AppointmentDate = new DateTime(2024 - 01 - 01)
            };

            _mockRepository.Setup(repo => repo.UpdateAppointmentAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOneAppointment(appointmentId, model);

            // Assert
            var okResult = result as OkResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateOneAppointment_ReturnsNotFound_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var model = new AppointmentModel
            {
                Id = appointmentId,
                ConsultantName = "Consultant A",
                AppointmentDate = new DateTime(2024 - 01 - 01)
            };

            _mockRepository.Setup(repo => repo.UpdateAppointmentAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOneAppointment(Guid.Empty, model);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }
    }
}
