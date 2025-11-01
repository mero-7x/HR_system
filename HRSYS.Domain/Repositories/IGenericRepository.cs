using System.Linq.Expressions;

namespace HRSYS.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
        Task<T?> GetByIdAsync(int id  , CancellationToken ct);
        IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate ,CancellationToken ct);
        Task AddAsync(T entity , CancellationToken ct);
        Task UpdateAsync(T entity , CancellationToken ct);
        Task DeleteAsync(T entity , CancellationToken ct);
    }
}
