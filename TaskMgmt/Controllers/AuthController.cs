using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMgmt.DTO;
using TaskMgmt.Service;

namespace TaskMgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto req)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.RegisterAsync(req);

            if (token is null) 
                return BadRequest("User already exists.");

            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto req)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.LoginAsync(req);

            if (token is null) 
                return Unauthorized("Invalid email or password.");

            return Ok(new { Token = token });
        }
    }
}
