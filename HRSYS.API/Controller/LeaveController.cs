using HRSYS.Application.DTOs;
using HRSYS.Application.Services;
using HRSYS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSYS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _service;

        public LeavesController(LeaveService service)
        {
            _service = service;
        }

        // 🟢 1) يقدم الموظف طلب إجازة
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveReqDto dto , CancellationToken ct)
        {
            var leave = await _service.CreateLeaveAsync(dto,  ct);
            return Ok(leave);
        }

     
        // [HttpGet]
        // [Authorize(Roles = "HR,Manager")]
        // public async Task<IActionResult> GetLeaves([FromQuery] LeavePaginationParams query, CancellationToken ct)
        // {
        //     var result = await _service.GetPagedAsync(query, ct);
        //     return Ok(result);
        // }

        
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveOrRejectLeave(int id, [FromBody] ApproveLeaveDto dto, CancellationToken ct)
        {
            
            dto.Id = id;

            
            // var mgrClaim =
            //     User.FindFirst("EmployeeId") ??
            //     User.FindFirst(ClaimTypes.NameIdentifier) ??
            //     User.FindFirst("id");

            // if (mgrClaim is null)
            //     return Unauthorized(new { message = "Missing manager id claim." });

            // if (!int.TryParse(mgrClaim.Value, out var managerId))
            //     return Unauthorized(new { message = "Invalid manager id claim." });

            
            await _service.ApproveOrRejectLeave( dto, ct);


            return Ok(new { message = "its done" });
        }
    }
}
