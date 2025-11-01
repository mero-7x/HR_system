using AutoMapper;
using BCrypt.Net;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Repositories;
using HRSYS.Application.DTOs;
using HRSYS.Application.Mapping;
using System.Security.Authentication;
using HRSYS.Application.Exceptions;
using HRSYS.Domain.Enums;
using System.Reflection;
using System.Net.Http.Headers;



namespace HRSYS.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmployeeRepository _empRepo;
        private readonly IDepartmentRepository _Deprepo;




        public UserService(IUserRepository userRepo, IJwtService jwtService, IUnitOfWork unitofWork, IEmployeeRepository empRepo, IDepartmentRepository Deprepo)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _unitofWork = unitofWork;
            _empRepo = empRepo;
            _Deprepo = Deprepo;


        }


        public async Task<string> RegisterAsync(UserRegisterDto dto, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(dto.username) || string.IsNullOrEmpty(dto.password))
            {
                throw new ArgumentException("Username and password are required");
            }
            var existingUser = await _userRepo.GetByUsernameAsync(dto.username, ct);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");

            }


            var user = AuthMapper.ToEntity(dto);

            await _userRepo.AddAsync(user, ct);

            return ("User registered successfully");
        }


        public async Task<UserDto> LoginAsync(UserLoginDto dto, CancellationToken ct)
        {

            var username = dto.username.Trim().ToLower();

            // Get user securely
            var user = await _userRepo.GetByUsernameAsync(username, ct);


            bool fakeCheck = BCrypt.Net.BCrypt.Verify("fake_password",
                BCrypt.Net.BCrypt.HashPassword("fake_password"));

            if (user == null || string.IsNullOrEmpty(user.Password))
            {

                BCrypt.Net.BCrypt.Verify(dto.password, BCrypt.Net.BCrypt.HashPassword("fake_password"));
                throw new HRSYS.Application.Exceptions.AuthenticationException("Invalid username or password");
            }


            if (!user.IsActive)
                throw new AccountDisabledException("Account is deactivated. Contact HR.");


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.password, user.Password);
            if (!isPasswordValid)
            {

                throw new HRSYS.Application.Exceptions.AuthenticationException("Invalid username or password");
            }
            var token = _jwtService.GenerateToken(user);
            return AuthMapper.ToLoginDto(dto, token);


        }

        public async Task<EmployeeDto> ActivateUserAsync(ActivateUserDto dto, CancellationToken ct)
        {
            var user = await _userRepo.GetByIdWithEmployeeAsync(dto.UserId, ct)
                ?? throw new KeyNotFoundException("User not found");

            var dep = await _Deprepo.GetByIdAsync(dto.DepartmentId, ct)
                ?? throw new KeyNotFoundException("Department not found");

            if (user.IsActive == true)
                throw new ValidationException("User already active.");

            if (dep.ManagerEmployeeId.HasValue && dep.ManagerEmployeeId != user.EmployeeId && dto.Role == Role.Manager)
                throw new ValidationException("This department already has a manager assigned.");

            if (user.Employee != null && user.Employee.DepartmentId == dto.DepartmentId)
                throw new ValidationException("User is already assigned to this department.");



            await _unitofWork.BeginTransactionAsync(ct);
            try
            {
                var employee = EmployeeMapper.MapToEntity(dto);
                await _empRepo.AddAsync(employee, ct);

                user.EmployeeId = employee.Id;
                user.Role = dto.Role;
                user.IsActive = true;
                await _userRepo.UpdateAsync(user, ct);


                // if (dto.Role == Role.Manager)
                // {

                //     dep.ManagerId = user.Id;
                //     dep.ManagerEmployeeId = employee.Id;
                //     await _Deprepo.UpdateAsync(dep, ct);

                //     var employeesInDep = await _empRepo.GetByDepartmentIdAsync(dto.DepartmentId, ct);
                //     foreach (var emp in employeesInDep)
                //     {
                //         emp.ManagerId = employee.Id;
                //         await _empRepo.UpdateAsync(emp, ct);
                //     }
                // }
                // else if (dto.Role == Role.Employee)
                // {
                //     if (dep.ManagerEmployeeId.HasValue)
                //         employee.ManagerId = dep.ManagerEmployeeId.Value;

                //     await _empRepo.UpdateAsync(employee, ct);
                // }

                if (dto.Role == Role.Employee)
                {
                    if (dep.ManagerEmployeeId.HasValue)
                        employee.ManagerId = dep.ManagerEmployeeId.Value;

                    await _empRepo.UpdateAsync(employee, ct);
                }

                else if (dto.Role == Role.HR)
                {
                    await _empRepo.UpdateAsync(employee, ct);
                }

                await _unitofWork.SaveChangesAsync(ct);
                await _unitofWork.CommitAsync(ct);


                return new EmployeeDto
                {
                    id = employee.Id,
                    FullName = dto.fullname,
                    DepartmentId = dto.DepartmentId,
                    BaseSalary = dto.BaseSalary,
                    ServiceInYears = dto.ServiceInYears,
                    Gender = dto.Gender,
                    CurrentDegree = dto.Degree,
                    Role = dto.Role,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,

                };
            }
            catch
            {
                await _unitofWork.RollbackAsync(ct);
                throw;
            }
        }

        public async Task AssignNewManagerAsync(ChangeManagerDto dto, CancellationToken ct)
        {
            var dep = await _Deprepo.GetByIdAsync(dto.DepartmentId, ct)
                ?? throw new KeyNotFoundException("Department not found.");

            await _unitofWork.BeginTransactionAsync(ct);

            try
            {
                if (dto.NewUserId == dep.ManagerId && dto.NewEmployeeId == dep .ManagerEmployeeId)
                    throw new ValidationException(" its alruday manager");
                
                    
                

                if (dep.ManagerEmployeeId.HasValue)
                {
                    var oldManagerEmpId = dep.ManagerEmployeeId.Value;
                    var oldManager = await _empRepo.GetByIdWithUserAsync(oldManagerEmpId, ct);

                    if (oldManager?.User != null)
                    {
                        oldManager.User.Role = Role.Employee;
                        await _userRepo.UpdateAsync(oldManager.User, ct);

                        oldManager.ManagerId = null;
                        await _empRepo.UpdateAsync(oldManager, ct);
                    }
                }

                dep.ManagerId = dto.NewUserId;
                dep.ManagerEmployeeId = dto.NewEmployeeId;
                await _Deprepo.UpdateAsync(dep, ct);

                var newManagerUser = await _userRepo.GetByIdAsync(dto.NewUserId, ct);
                if (newManagerUser == null)
                    throw new KeyNotFoundException("New manager user not found.");

                newManagerUser.Role = Role.Manager;
                await _userRepo.UpdateAsync(newManagerUser, ct);

                var employeesInDep = await _empRepo.GetByDepartmentIdAsync(dto.DepartmentId, ct);
                foreach (var emp in employeesInDep)
                {
                    emp.ManagerId = emp.Id == dto.NewEmployeeId ? null : dto.NewEmployeeId;
                    await _empRepo.UpdateAsync(emp, ct);
                }

                await _unitofWork.SaveChangesAsync(ct);
                await _unitofWork.CommitAsync(ct);
            }
            catch
            {
                await _unitofWork.RollbackAsync(ct);
                throw;
            }
        }



        public async Task<IEnumerable<UserGetAllDto>> GetAllUsers(CancellationToken ct)
        {
            var user = await _userRepo.GetAllAsync(ct);
            return user.Select(u => UserMapper.ToDto(u));
        }


    }
}
