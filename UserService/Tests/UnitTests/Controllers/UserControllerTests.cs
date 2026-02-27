using Api.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace UnitTests.Controllers
{

    public class UserControllerTests
    {
        private readonly Mock<IUserService> _serviceMock = new();

        [Fact]
        public async Task MyDetails_Should_ReturnOk_When_UserExists()
        {
            var email = "test@test.com";

            var dto = new UserDTO
            {
                Email = email
            };

            _serviceMock.Setup(s => s.GetCurrentUserAsync(email)).ReturnsAsync(dto);

            var controller = CreateController(email);

            var result = await controller.MyDetails();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(email, returnedValue.Email);
        }

        [Fact]
        public async Task MyDetails_Should_ReturnUnauthorized_When_EmailMissing()
        {
            var controller = CreateController(null);

            var result = await controller.MyDetails();

            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }

        [Fact]
        public async Task MyDetails_Should_ReturnNotFound_When_UserNotFound()
        {
            _serviceMock.Setup(s => s.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync((UserDTO?)null);

            var controller = CreateController("none@test.com");

            var result = await controller.MyDetails();

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        private UserController CreateController(string? email)
        {
            var controller = new UserController(_serviceMock.Object);
            var claims = email != null ? [new Claim(ClaimTypes.Email, email)] : Array.Empty<Claim>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }
    }
}
