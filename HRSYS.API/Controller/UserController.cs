using HRSYS.Application.DTOs;
using HRSYS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSYS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "HR")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        // add Employee
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateUserDto dto, CancellationToken ct)
        {
            var emp = await _userService.ActivateUserAsync(dto, ct);

            return Ok(new
            {
                message = "User activated and employee created successfully",
                data = emp
            });

        }

        [HttpPut("change-manager")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ChangeManager([FromBody] ChangeManagerDto dto, CancellationToken ct)
        {
            await _userService.AssignNewManagerAsync(dto, ct);
            return Ok(new { message = "Manager changed successfully!", dto.Reason });
        }



        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(CancellationToken ct)
        {
            var user = await _userService.GetAllUsers(ct);
            return Ok(user);
        }
    }
}
