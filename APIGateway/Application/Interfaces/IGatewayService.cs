using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGatewayService
    {
        Task<string> RegisterAsync(RegisterDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginDTO request);
        Task<UserDTO> GetMyDetailsAsync(string token);
    }
}
