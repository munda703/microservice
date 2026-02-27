using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> GetCurrentUserAsync(string email);
    }
}