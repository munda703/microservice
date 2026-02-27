using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper,ILogger<UserService> logger) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<UserDTO?> GetCurrentUserAsync(string email)
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                return null;
            }
            
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<User> LoginAsync(LoginDTO request)
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(request.Email.ToLower());

            if (user == null)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return user;
        }
    }
}
