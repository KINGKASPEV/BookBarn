using System.Linq.Expressions;

namespace BookBarn.Infrastructure.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllToIncludeAsync(params Expression<Func<T, object>>[] includes);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> FindAndIncludeAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(List<T> entities);
        Task<int> SaveChangesAsync();
        Task<T> FindSingleAsync(Expression<Func<T, bool>> expression);
    }
}
