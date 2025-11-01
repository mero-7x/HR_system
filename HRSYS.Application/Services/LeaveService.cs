using AutoMapper;
using BCrypt.Net;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Repositories;
using HRSYS.Application.DTOs;
using HRSYS.Application.Mapping;
using System.Security.Authentication;
using HRSYS.Application.Exceptions;
using HRSYS.Domain.Enums;


namespace HRSYS.Application.Services
{
    public class LeaveService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmployeeRepository _empRepo;
        private readonly IDepartmentRepository _Deprepo;
        private readonly ILeaveRepository _LeaveRepo;
        private readonly ICurrentUserService _currentUser;




        public LeaveService(IUserRepository userRepo, IJwtService jwtService, IUnitOfWork unitofWork, IEmployeeRepository empRepo, IDepartmentRepository Deprepo, ILeaveRepository LeaveRepo, ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _unitofWork = unitofWork;
            _empRepo = empRepo;
            _Deprepo = Deprepo;
            _LeaveRepo = LeaveRepo;
            _currentUser = currentUser;

        }


        public async Task<LeaveDto> CreateLeaveAsync(LeaveReqDto dto, CancellationToken ct)
        {
            // 1️⃣ نحصل على المستخدم الحالي
            var userId = _currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("User ID not found in current context.");

            // 2️⃣ نجيب الموظف المرتبط بالمستخدم الحالي
            var emp = await _empRepo.GetByUserIdAsync(userId.Value, ct);
            if (emp == null)
                throw new KeyNotFoundException("Employee not found for this user.");

            // 3️⃣ نتحقق من حالة الحساب
            if (emp.User == null || !emp.User.IsActive)
                throw new InvalidOperationException("Inactive users cannot request leave.");

            // 4️⃣ نتحقق من التواريخ
            if (dto.StartDate.Date >= dto.EndDate.Date)
                throw new ArgumentException("End date must be after start date.");

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Start date cannot be in the past.");

            // 5️⃣ نتحقق أن الموظف عنده مدير فعلي
            if (emp.ManagerId == null)
                throw new InvalidOperationException("This employee does not have an assigned manager.");

            var manager = await _empRepo.GetByIdWithUserAsync(emp.ManagerId.Value, ct);
            if (manager == null || manager.User == null || !manager.User.IsActive)
                throw new InvalidOperationException("Assigned manager is invalid or inactive.");

          
            // if (emp.DepartmentId == null)
            //     throw new InvalidOperationException("Employee is not assigned to any department.");

            var leave = LeaveMapper.ToEntity(dto, ct);
            leave.EmployeeId = emp.Id;
            leave.Status = LeaveStatus.Pending; 

            await _LeaveRepo.AddAsync(leave, ct);
            await _unitofWork.SaveChangesAsync(ct);

            return LeaveMapper.ToDto(leave, ct);
        }


        // public async Task<bool> ApprovalLeave(ApproveLeaveDto id, CancellationToken ct)
        // {
        //     var leave = await _LeaveRepo.GetByIdAsync(id.Id, ct);
        //     if (leave == null)
        //         throw new KeyNotFoundException(" Leave not found");

        //     leave.Status = id.Status;
        //     await _LeaveRepo.UpdateAsync(leave, ct);
        //     await _unitofWork.SaveChangesAsync(ct);

        //     return true;
        // }


        public async Task<bool> ApproveOrRejectLeave(ApproveLeaveDto dto, CancellationToken ct)
        {

            var leave = await _LeaveRepo.GetByIdAsync(dto.Id, ct) ?? throw new KeyNotFoundException("leave request not found");
            var employee = await _empRepo.GetByIdAsync(leave.EmployeeId, ct) ?? throw new KeyNotFoundException("Employee not found");
            var managerid = _currentUser.UserId;
            var managerrole = _currentUser.Role;
            if (managerrole != Role.Manager)
                throw new Exception(" its not manager");

            if (employee.ManagerId != managerid) throw new UnauthorizedAccessException("Not your subordinate");
            if (leave.Status != LeaveStatus.Pending) throw new NotFoundException(" its status is not pending");
            if (leave.Status == LeaveStatus.Rejected) throw new NotFoundException(" Already rejected by manager");

            leave.Status = dto.Status;

            if (dto.Status == LeaveStatus.Rejected)
            {
                if (string.IsNullOrWhiteSpace(dto.RejectReason))
                    throw new InvalidOperationException("Rejection reason is required when rejecting a leave request.");


                leave.ManagerComments = dto.RejectReason;
              
                leave.ManagerRejectDate = DateTime.UtcNow;
                leave.ByManagerId = managerid;
            }
            else
            {
               
                leave.ByManagerId = managerid;
                leave.ManagerApprovalDate = DateTime.UtcNow; 

            }

            await _LeaveRepo.UpdateAsync(leave, ct);
            await _unitofWork.SaveChangesAsync(ct);
            return true;
        }


    }





}
