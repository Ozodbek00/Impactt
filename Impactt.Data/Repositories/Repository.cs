using Impactt.Data.DbContexts;
using Impactt.Data.Helpers;
using Impactt.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Impactt.Data.Repositories
{
    public class Repository<TSource> : IRepository<TSource> where TSource : class
    {
        protected readonly AppDbContext dbContext;
        protected readonly DbSet<TSource> dbSet;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TSource>();
        }

        /// <summary>
        /// Add entity to db.
        /// </summary>
        public async Task<TSource> AddAsync(TSource entity)
        {
            var entry = await dbSet.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return entry.Entity;
        }

        /// <summary>
        /// Get entities with pagination.
        /// </summary>
        public IQueryable<TSource> GetAll(int pageIndex, int pageSize, Expression<Func<TSource, bool>> expression = null,
                    string[] includes = null, bool isTracking = true)
        {
            IQueryable<TSource> query = expression is null ? dbSet : dbSet.Where(expression);

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (!isTracking)
                query = query.AsNoTracking();

            return query.Paginate(pageIndex, pageSize);
        }

        /// <summary>
        /// Get entity from db by condition.
        /// </summary>
        public async Task<TSource> GetAsync(Expression<Func<TSource, bool>> expression, string[] includes = null)
        {
            IQueryable<TSource> query = expression is null ? dbSet : dbSet.Where(expression);

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        public async Task<TSource> UpdateAsync(TSource entity)
        {
            var result = dbSet.Update(entity);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        public async Task DeleteAsync(TSource entity)
        {
            dbSet.Remove(entity);

            await dbContext.SaveChangesAsync();
        }
    }
}
