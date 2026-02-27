using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IConfiguration config, ILogger<AuthService> logger, IMapper mapper) : IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _config = config;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<User> RegisterAsync(RegisterDTO request)
        {
            request.Email = request.Email.ToLower();
            var existing = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);

            if (existing.Any())
            {
                _logger.LogWarning("Registration failed. Email already exists: {Email}", request.Email);
                throw new InvalidOperationException("Email already exists");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("User registered successfully: {Email}", user.Email);

            return user;
        }

    }
}
