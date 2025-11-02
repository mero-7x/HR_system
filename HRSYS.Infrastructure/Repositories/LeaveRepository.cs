using HRSYS.Domain.Repositories;

using HRSYS.Domain.Entities;
using HRSYS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



namespace HRSYS.Infrastructure.Repositories

{
    public class LeaveRepository : GenericRepository<Leave>, ILeaveRepository

    {
        public LeaveRepository(AppDbContext context) : base(context) { }
        public async Task<List<Leave>> GetLeavesByEmployeeIdAsync(int empid, CancellationToken ct)
        {
            return await _context.Leaves
                .Include(i => i.Employee)
                .Where(l => l.EmployeeId == empid)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Leave>> GetAllWithEmployeeAsync(CancellationToken ct)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.User)
                .ToListAsync(ct);
        }


    }

}