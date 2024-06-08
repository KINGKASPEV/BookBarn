using BookBarn.Infrastructure.Context;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookBarn.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookBarnDbContext _dbContext;

        public GenericRepository(BookBarnDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);

        public void DeleteAll(List<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression) => await _dbContext.Set<T>().Where(expression).ToListAsync();
        public async Task<List<T>> FindAndIncludeAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.Where(expression).ToListAsync();
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression) => await _dbContext.Set<T>().FirstOrDefaultAsync(expression);

        public async Task<List<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
        public async Task<List<T>> GetAllToIcludeAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id) => await _dbContext.Set<T>().FindAsync(id);

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);
    }
}
