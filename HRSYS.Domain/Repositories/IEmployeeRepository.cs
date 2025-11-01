using HRSYS.Domain.Entities;
using System.Collections.Generic;
using HRSYS.Domain.Repositories;

namespace HRSYS.Domain.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // دوال إضافية تخص الموظف فقط
        Task<IEnumerable<Employee>> GetEmployeesWithDepartmentAsync();
        IQueryable<Employee> Query();
        Task<IEnumerable<Employee>> GetAllWithIncludesAsync(CancellationToken ct);

        Task<Employee?> GetByUserIdAsync(int userid, CancellationToken ct);
        Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int depId, CancellationToken ct);
         Task<Employee?> GetByIdWithUserAsync(int id, CancellationToken ct);

        

        
    }
}
