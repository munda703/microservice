
using ApiGateway.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers
{
    public class GatewayControllerTests
    {
        private readonly Mock<IGatewayService> _serviceMock = new();

        [Fact]
        public async Task Login_Should_ReturnOk_WithToken()
        {
            var request = new LoginDTO();
            var response = new AuthResponseDTO
            {
                AccessToken = "token"
            };

            _serviceMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

            var controller = new GatewayController(_serviceMock.Object);

            var result = await controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<AuthResponseDTO>(okResult.Value);
            Assert.Equal("token", returnedValue.AccessToken);
        }
    }
}
