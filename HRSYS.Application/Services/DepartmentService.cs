using HRSYS.Application.DTOs;
using HRSYS.Application.Exceptions;
using HRSYS.Application.Mapping;
using HRSYS.Domain.Repositories;

namespace HRSYS.Application.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository _Deprepo;

        public DepartmentService(IDepartmentRepository Deprepo)
        {
            _Deprepo = Deprepo;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken ct)
        {
            var deps = await _Deprepo.GetAllAsync(ct);
            return deps.Select(DepartmentMapper.ToDto);
        }

        public async Task<DepartmentDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var dep = await _Deprepo.GetByIdAsync(id, ct);
            if (dep == null)
                throw new NotFoundException("Department not found");
            return DepartmentMapper.ToDto(dep);
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto, CancellationToken ct)
        {
            var existing = await _Deprepo.GetByNameAsync(dto.Name, ct);
            if (existing != null)
                throw new ValidationException("Department already exists");

            var dep = DepartmentMapper.ToEntity(dto);
            await _Deprepo.AddAsync(dep, ct);
            return DepartmentMapper.ToDto(dep);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var dep = await _Deprepo.GetByIdAsync(id, ct);
            if (dep == null)
                throw new NotFoundException("Department not found");

            await _Deprepo.DeleteAsync(dep, ct);
            return true;
        }
    }
}
