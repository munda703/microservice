using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<User> RegisterAsync(RegisterDTO request);
    }
}
