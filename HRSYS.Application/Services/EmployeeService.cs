using HRSYS.Application.DTOs.Pagination;
using HRSYS.Application.DTOs;
// using HRSYS.Application.Interfaces;
using HRSYS.Domain.Repositories;
using HRSYS.Application.Mapping;
using HRSYS.Application.Exceptions;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HRSYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HRSYS.Domain.Enums;




namespace HRSYS.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ILeaveRepository _Leaverepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDepartmentRepository _departmentrepo;
        private readonly IUserRepository _userRepo;
        private IUnitOfWork _unitOfWork;




        public EmployeeService(IEmployeeRepository employeeRepo, IHttpContextAccessor httpContextAccessor, ILeaveRepository Leaverepo, IDepartmentRepository department, IUserRepository user, IUnitOfWork unitOfWork)
        {
            _employeeRepo = employeeRepo;
            _httpContextAccessor = httpContextAccessor;
            _Leaverepo = Leaverepo;
            _departmentrepo = department;
            _userRepo = user;
            _unitOfWork = unitOfWork;

        }

        public async Task<PagedResponse<EmployeeDto>> GetPagedAsync(PaginationParams query, CancellationToken ct)
        {
            var employees =  _employeeRepo.Query();
            var filtered = employees.AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchTerm = query.Search.Trim().ToLower();
                filtered = filtered.Where(e => e.FullName.ToLower().Contains(searchTerm));
            }


            if (query.DepartmentId.HasValue)
                filtered = filtered.Where(e => e.DepartmentId == query.DepartmentId.Value);


            if (query.ManagerId.HasValue)
                filtered = filtered.Where(e => e.ManagerId == query.ManagerId.Value);


            if (query.IsActive.HasValue)
                filtered = filtered.Where(e => e.User != null && e.User.IsActive == query.IsActive.Value);


            if (query.Role.HasValue)
                filtered = filtered.Where(e => e.User.Role == query.Role.Value);

            // filtered = filtered.Where(e => e.User != null && e.User.Role == query.Role.Value);



            var sortDesc = query.Desc;
            switch (query.SortBy?.ToLower())
            {
                case "fullname":
                    filtered = sortDesc ? filtered.OrderByDescending(e => e.FullName) : filtered.OrderBy(e => e.FullName);
                    break;
                case "department":
                case "departmentname":
                    filtered = sortDesc
                        ? filtered.OrderByDescending(e => e.Department == null ? "" : e.Department.Name)
                        : filtered.OrderBy(e => e.Department == null ? "" : e.Department.Name);
                    break;
                case "role":
                    filtered = sortDesc
                        ? filtered.OrderByDescending(e => e.User == null ? "" : e.User.Role.ToString())
                        : filtered.OrderBy(e => e.User == null ? "" : e.User.Role.ToString());
                    break;
                case "isactive":
                    filtered = sortDesc
                        ? filtered.OrderByDescending(e => e.User == null ? false : e.User.IsActive)
                        : filtered.OrderBy(e => e.User == null ? false : e.User.IsActive);
                    break;
                default:
                    filtered = sortDesc ? filtered.OrderByDescending(e => e.Id) : filtered.OrderBy(e => e.Id);
                    break;
            }


            var totalCount = await filtered.CountAsync();
            var pagedData =   await filtered
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = pagedData.Select(EmployeeMapper.ToDto);
            
                  

            return new PagedResponse<EmployeeDto>(
                totalCount: totalCount,
                pageNumber: query.Page,
                pageSize: query.PageSize,
                data: result
            );
        }





        public async Task<EmployeeDto> GetEmpByIDAsync(int id, CancellationToken ct)
        {
            var emp = await _employeeRepo.GetByIdAsync(id, ct);
            if (emp == null) throw new NotFoundException("Employee not found");
            return EmployeeMapper.ToDto(emp);
        }

        public async Task<IEnumerable<LeaveDto>> GetMyLeavesAsync(int empid, CancellationToken ct)

        {
            var empId = _httpContextAccessor.HttpContext?.User?
                 .FindFirst("EmployeeId")?.Value;

            var role = _httpContextAccessor.HttpContext?.User?
               .FindFirst(ClaimTypes.Role)?.Value;

            if (role != "Employee")
                throw new UnauthorizedAccessException("Only employees can view their leaves.");

            if (string.IsNullOrEmpty(empId))
                throw new NotFoundException("Employee ID not found in token.");

            var leaves = await _Leaverepo.GetLeavesByEmployeeIdAsync(int.Parse(empId), ct);


            var leaveDto = leaves.Select(l => LeaveMapper.ToDto(l , ct));

            return leaveDto;
        }
        // post a employee


        // public async Task<EmployeeDto> CreateEmployeeWithUserAsync(EmployeeCreateDto dto, CancellationToken ct)
        // {
        //     var department = await _departmentrepo.GetByIdAsync(dto.DepartmentId, ct);
        //     if (department == null)
        //         throw new NotFoundException("Department not found");

        //     var existingUser = await _userRepo.GetByUsernameAsync(dto.Username, ct);
        //     if (existingUser != null)
        //         throw new ValidationException("Username is already taken.");




        //     await _unitOfWork.BeginTransactionAsync(ct);
        //     try
        //     {
        //         var employee = EmployeeMapper.ToEntity(dto);
        //         await _employeeRepo.AddAsync(employee, ct);


        //         await _unitOfWork.SaveChangesAsync(ct);
        //         var user = EmployeeMapper.ToEntityUser(dto, employee.Id);
        //         await _userRepo.AddAsync(user, ct);
        //         await _unitOfWork.SaveChangesAsync(ct);
        //         await _unitOfWork.CommitAsync(ct);
        //         return EmployeeMapper.ToDto(employee);

        //     }
        //     catch
        //     {
        //         await _unitOfWork.RollbackAsync(ct);
        //         throw;
        //     }


        // }

        //  Update Employee
        public async Task<EmployeeDto> UpdateEmployeeAsync(int Id, EmployeeUpdateDto dto, CancellationToken ct)
        {
            var emp = await _employeeRepo.GetByIdAsync(Id, ct);
            if (emp == null)
                throw new NotFoundException($"Employee with ID {Id} not found");


            if (dto.DepartmentId != emp.DepartmentId)
            {
                var dep = await _departmentrepo.GetByIdAsync(dto.DepartmentId, ct);
                if (dep == null)
                    throw new ValidationException("Invalid Department selected.");
            }

            emp.FullName = dto.FullName.Trim();
            emp.DepartmentId = dto.DepartmentId;
            emp.BaseSalary = dto.BaseSalary;
            emp.ServiceInYears = dto.ServiceInYears;



            await _employeeRepo.UpdateAsync(emp, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return EmployeeMapper.ToDto(emp);
        }


        //  Soft Delete Employee (Deactivate instead of delete)
        public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken ct)
        {
            var emp = await _employeeRepo.GetByIdAsync(id, ct);
            if (emp == null) throw new NotFoundException("User not found");

            var user = await _userRepo.GetByEmployeeIdAsync(emp.Id, ct);
            if (user == null) throw new KeyNotFoundException("user not found");

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                user.IsActive = false;
                await _userRepo.UpdateAsync(user, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitAsync(ct);


            }
            catch

            {
                await _unitOfWork.RollbackAsync(ct);
                throw;
            }


            return true;
        }



    }
}

