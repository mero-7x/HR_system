
using HRSYS.Application.DTOs;
using HRSYS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSYS.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto ,  CancellationToken ct)
        {
            var userDto = await _userService.LoginAsync(dto ,ct);
            return Ok(userDto);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto  ,  CancellationToken ct)
        {
            var message = await _userService.RegisterAsync(dto  ,ct);
            return Ok(new { message });
        }
    }
}