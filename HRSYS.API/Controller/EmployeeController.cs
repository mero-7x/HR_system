
using Microsoft.AspNetCore.Mvc;
using HRSYS.Application.Services;
using HRSYS.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using HRSYS.Application.DTOs.Pagination;

namespace HRSYS.API.Controllers

{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [Authorize(Roles = "HR")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams query, CancellationToken ct)
        {
            var result = await _employeeService.GetPagedAsync(query, ct);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpById(int id, CancellationToken ct)
        {
            var emp = await _employeeService.GetEmpByIDAsync(id, ct);

            return Ok(emp);
        }

        // ✅ 3. Get my own leaves (requires Employee role)
        [HttpGet("my-leaves")]
        [Authorize(Roles = "Employee , HR")]
        public async Task<IActionResult> GetMyLeaves(CancellationToken ct)
        {
            var leaves = await _employeeService.GetMyLeavesAsync(0, ct);
            return Ok(leaves);
        }


        [Authorize(Roles = "HR,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto dto, CancellationToken ct)
        {
            var updated = await _employeeService.UpdateEmployeeAsync(id, dto, ct);

            return Ok(new
            {
                message = " Employee updated successfully",
                employee = updated
            });
        }


    }




}


