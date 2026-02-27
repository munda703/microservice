using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/gateWay")]
    public class GatewayController(IGatewayService service) : ControllerBase
    {
        private readonly IGatewayService _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            var res = await _service.RegisterAsync(request);
            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO request)
        {
            var res = await _service.LoginAsync(request);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("mydetails")]
        public async Task<ActionResult<UserDTO>> MyDetails()
        {
            var token = Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            var result = await _service.GetMyDetailsAsync(token);

            return Ok(result);
        }
    }
}
