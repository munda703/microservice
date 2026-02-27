using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<T> Update(T entity);
        void Delete(T entity);
    }
}
