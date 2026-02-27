using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [Authorize]
        [HttpGet("myDetails")]
        public async Task<ActionResult<UserDTO>> MyDetails()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Invalid token: email claim missing.");

            var user = await _service.GetCurrentUserAsync(email);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO request)
        {
            var res = await _service.LoginAsync(request);
            return Ok(res);
        }

    }
}
