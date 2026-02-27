using Api.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers
{
    public class RegistrationControllerTests
    {
        private readonly Mock<IRegistrationService> _serviceMock = new();

        [Fact]
        public async Task Register_Should_ReturnOk_WithToken()
        {
            var request = new RegisterDTO()
            {
                Password = "password",
                Email = "Mandisa.Nomda@gmail.com"
            };
            var response = new Domain.Entities.User()
            {
                Email = request.Email,
                LastName = request.LastName,
            };

            _serviceMock.Setup(s => s.RegisterAsync(request)).ReturnsAsync(response);

            var controller = new RegistrationController(_serviceMock.Object);

            var result = await controller.Register(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<User>(okResult.Value);
        }
    }
}
