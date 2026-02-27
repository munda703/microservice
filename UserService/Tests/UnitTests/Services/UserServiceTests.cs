using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<UserService>> _loggerMock = new();

        [Fact]
        public async Task GetCurrentUserAsync_Should_ReturnUserDTO_When_UserExists()
        {
            var email = "test@ufh.ac.za";

            var user = new User
            {
                Email = email
            };

            var userDto = new UserDTO
            {
                Email = email
            };

            _unitOfWorkMock.Setup(x => x.Users.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            _mapperMock.Setup(x => x.Map<UserDTO>(user)).Returns(userDto);

            var service = CreateService();

            var result = await service.GetCurrentUserAsync(email);

            Assert.NotNull(result);
            Assert.Equal(email, result!.Email);
        }

        [Fact]
        public async Task GetCurrentUserAsync_Should_ReturnNull_When_UserDoesNotExist()
        {
            _unitOfWorkMock.Setup(x => x.Users.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var service = CreateService();

            var result = await service.GetCurrentUserAsync("test@ufh.ac.za");

            Assert.Null(result);
        }

        private UserService CreateService()
        {
            return new UserService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }
    }
}