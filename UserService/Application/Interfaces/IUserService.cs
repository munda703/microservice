using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> GetCurrentUserAsync(string email);
        Task<User> LoginAsync(LoginDTO request);
    }
}