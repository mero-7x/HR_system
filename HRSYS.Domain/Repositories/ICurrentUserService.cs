using HRSYS.Domain.Enums;

namespace HRSYS.Domain.Repositories
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? EmployeeId { get; }
        string? Username { get; }
        Role? Role { get; }
        bool IsAuthenticated { get; }
    }
}
