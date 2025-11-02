using HRSYS.Application.DTOs;
using HRSYS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSYS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentController(DepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
         [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateDto dto, CancellationToken ct)
        {
            var result = await _service.CreateAsync(dto, ct);
            return Ok(result);

        }



        [HttpDelete("{id}")]
         [Authorize(Roles = "HR")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return Ok("Department Delete succses");
        }
    }
}
