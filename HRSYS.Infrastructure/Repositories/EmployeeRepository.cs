using HRSYS.Domain.Repositories;

using HRSYS.Domain.Entities;
using HRSYS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRSYS.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetEmployeesWithDepartmentAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public IQueryable<Employee> Query()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.User)
                .AsQueryable();
        }

        public async Task<IEnumerable<Employee>> GetAllWithIncludesAsync(CancellationToken ct)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.User)
                .ToListAsync(ct);
        }


        public async Task<Employee?> GetByUserIdAsync(int userid, CancellationToken ct)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.User != null && e.User.Id == userid, ct);

        }

        public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int depId, CancellationToken ct)
        {
            return await _context.Employees.Where(e => e.DepartmentId == depId).ToListAsync(ct);

        }
        
          public async Task<Employee?> GetByIdWithUserAsync(int id, CancellationToken ct)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

       
    }


}

