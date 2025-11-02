using HRSYS.Application.DTOs;
using HRSYS.Application.Services;
using HRSYS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRSYS.Domain.Enums;

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

      
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveReqDto dto, CancellationToken ct)
        {
            var leave = await _service.CreateLeaveAsync(dto, ct);
            return Ok(leave);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveOrRejectLeave(int id, [FromBody] ApproveLeaveDto dto, CancellationToken ct)
        {

            dto.Id = id;
            await _service.ApproveOrRejectLeave(dto, ct);


            return Ok(new { message = "its done" });
        }

        [HttpGet]
        [Authorize(Roles = "HR, Manager")]
        public async Task<IActionResult> GetLeaves([FromQuery] LeaveStatus? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            var result = await _service.GetLeavesAsync(status, pageNumber, pageSize, ct);
            return Ok(result);
        }

    }
}
