using HRSYS.Domain.Entities;

namespace HRSYS.Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department?> GetByNameAsync(string name, CancellationToken ct);
    }
}
