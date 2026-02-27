using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Services
{
    public class RegistrationServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<AuthService>> _loggerMock = new();
        private readonly IConfiguration _configuration;

        public RegistrationServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Secret", "THIS_IS_A_SUPER_SECRET_KEY_12345" }
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!).Build();
        }

        [Fact]
        public async Task RegisterAsync_Should_ReturnToken_When_EmailNotExists()
        {
            var request = new RegisterDTO
            {
                Email = "test@ufh.ac.za",
                Password = "1234"
            };

            _unitOfWorkMock.Setup(x => x.Users.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync([]);

            _mapperMock.Setup(m => m.Map<User>(request)).Returns(new User { Email = request.Email });

            var service = CreateService();

            var result = await service.RegisterAsync(request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task RegisterAsync_Should_ThrowException_When_EmailExists()
        {
            var request = new RegisterDTO
            {
                Email = "test@ufh.ac.za",
                Password = "1234"
            };

            _unitOfWorkMock.Setup(x => x.Users.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync([new()]);

            var service = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync(request));
        }

        private AuthService CreateService()
        {
            return new AuthService(
                _unitOfWorkMock.Object,
                _configuration,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }
    }
}