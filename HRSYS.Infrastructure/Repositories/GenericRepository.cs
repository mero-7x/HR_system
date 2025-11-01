
using HRSYS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HRSYS.Domain.Repositories;


namespace HRSYS.Infrastructure.Repositories
{
    
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); 
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id ,  CancellationToken ct)
        {
            
            return await _dbSet.FindAsync(id);
        }

        public  IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate , CancellationToken ct)
        {
            return  _dbSet.Where(predicate).AsQueryable();
        }

        public async Task AddAsync(T entity , CancellationToken ct)
        {
            await _dbSet.AddAsync(entity , ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(T entity , CancellationToken ct)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(T entity , CancellationToken ct)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
