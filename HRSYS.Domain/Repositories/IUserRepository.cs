using HRSYS.Domain.Entities;

namespace HRSYS.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
        Task<User?> GetByEmployeeIdAsync(int employeeId, CancellationToken ct);
        Task<User?> GetByIdWithEmployeeAsync(int emp, CancellationToken ct);

        
    }
}
