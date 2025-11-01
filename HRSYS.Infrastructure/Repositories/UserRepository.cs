using HRSYS.Domain.Repositories;

using HRSYS.Domain.Entities;
using HRSYS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace HRSYS.Infrastructure.Repositories

{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
        {
            return await _context.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        public async Task<User?> GetByEmployeeIdAsync(int empId, CancellationToken ct)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.EmployeeId == empId, ct);
        }


        public async Task<User?> GetByIdWithEmployeeAsync(int id, CancellationToken ct)
        {
            return await _context.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }



    }
}