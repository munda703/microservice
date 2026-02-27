using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class RegistrationController(IRegistrationService service) : ControllerBase
    {
        private readonly IRegistrationService _service = service;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO request)
        {
            var res = await _service.RegisterAsync(request);
            return Ok(res);
        }
    }
}
