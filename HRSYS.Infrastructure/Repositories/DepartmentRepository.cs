
using HRSYS.Domain.Entities;
using HRSYS.Infrastructure.Data;
using HRSYS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace HRSYS.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context) { }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }


        public async Task<Department?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower(), ct);
        }
        

    }
}
   

