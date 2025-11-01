using HRSYS.Application.DTOs;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace HRSYS.Application.Mapping
{
    public static class AuthMapper
    {
        public static User ToEntity(UserRegisterDto request)
        {
            return new User
            {
                Username = request.username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.password),
                Role = Role.Pending


            };

        }


        public static UserDto ToLoginDto(UserLoginDto request, string token)
        {
            return new UserDto
            {
                Username = request.username,
                Token = token
            };
        }
    }

    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee employee)
        {
            return new EmployeeDto
            {
                id = employee.Id,
                FullName = employee.FullName,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name,
                BaseSalary = employee.BaseSalary,
                ServiceInYears = employee.ServiceInYears,
                Role = employee.User?.Role,
                Gender = employee.Gender,
                CurrentDegree = employee.CurrentDegree,
                CreatedAt = employee.CreatedAt,
                IsActive = employee.User?.IsActive ?? false



            };
        }
        // public static Employee ToEntity(EmployeeCreateDto dto)
        // {
        //     return new Employee
        //     {
        //         FullName = dto.FullName,
        //         DepartmentId = dto.DepartmentId,
        //         BaseSalary = dto.BaseSalary,
        //         ServiceInYears = dto.ServiceInYears,
        //         Gender = Enum.Parse<Gender>(dto.Gender, true),
        //         CurrentDegree = Enum.Parse<Degree>(dto.CurrentDegree, true),

        //     };
        // }

        public static User ToEntityUser(EmployeeCreateDto dto, int empid)
        {
            return new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Role.Employee,
                EmployeeId = empid,
                IsActive = true
            };
        }

        public static Employee MapToEntity(ActivateUserDto dto)
        {
            return new Employee
            {
                FullName = dto.fullname,                 // ناخذها من الـ User
                DepartmentId = dto.DepartmentId,
                BaseSalary = dto.BaseSalary,
                ServiceInYears = dto.ServiceInYears,
                Gender = dto.Gender,
                CurrentDegree = dto.Degree,
            };
        }


        public static EmployeeDto MapToDto(Employee entity)
        {
            return new EmployeeDto
            {
                id = entity.Id,
                FullName = entity.FullName,
                DepartmentId = entity.DepartmentId,
                BaseSalary = entity.BaseSalary,
                ServiceInYears = entity.ServiceInYears,
                Gender = entity.Gender,
                CurrentDegree = entity.CurrentDegree,
                Role = entity.User?.Role,
                CreatedAt = entity.CreatedAt,
                IsActive = entity.User?.IsActive
            };
        }

    }

    public static class LeaveMapper

    {
        public static LeaveDto ToDto(Leave leave, CancellationToken ct)
        {
            return new LeaveDto
            {
                Type = leave.Type,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Status = leave.Status,
                Reason = leave.Reason

            };
        }

        public static Leave ToEntity(LeaveReqDto leave, CancellationToken ct)
        {
            return new Leave
            {
                Type = leave.Type,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                // Status = leave.Status,
                Reason = leave.Reason

            };
        }

        //   public static RejectLeaveDto ToRejectDto(LeaveReqDto leave , CancellationToken ct)
        // {
        //     return new RejectLeaveDto
        //     {
        //         Type = leave.Type,
        //         StartDate = leave.StartDate,
        //         EndDate = leave.EndDate,
        //         // Status = leave.Status,
        //         Reason = leave.Reason

        //     };
        // }


    }



    public static class DepartmentMapper
    {
        public static DepartmentDto ToDto(Department dep) => new()
        {
            Id = dep.Id,
            Name = dep.Name,
            IsActive = dep.IsActive,
            CreatedAt = dep.CreatedAt

        };

        public static Department ToEntity(DepartmentCreateDto dto) => new()
        {
            Name = dto.Name,

        };
    }


    public static class UserMapper
    {
        public static UserGetAllDto ToDto(User user)
        {
            return new UserGetAllDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.Employee?.FullName ?? string.Empty,
                Role = user.Role.ToString(),
                IsActive = user.IsActive
            };
        }
    }

}
