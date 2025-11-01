namespace HRSYS.Domain.Repositories
{
    using HRSYS.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILeaveRepository : IGenericRepository<Leave>
    {
        
        Task<List<Leave>> GetLeavesByEmployeeIdAsync(int empd , CancellationToken ct);
    }
}