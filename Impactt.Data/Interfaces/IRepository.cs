using System.Linq.Expressions;

namespace Impactt.Data.Interfaces
{
    public interface IRepository<TSource> where TSource : class
    {
        /// <summary>
        /// Add entity to db.
        /// </summary>
        Task<TSource> AddAsync(TSource entity);

        /// <summary>
        /// Get entities with pagination.
        /// </summary>
        IQueryable<TSource> GetAll(int pageIndex, int pageSize, 
            Expression<Func<TSource, bool>> expression = null, 
            string[] includes = null, bool isTracking = true);

        /// <summary>
        /// Get entity from db by condition.
        /// </summary>
        Task<TSource> GetAsync(Expression<Func<TSource, bool>> expression,
                                                        string[] includes = null);

        /// <summary>
        /// Update entity.
        /// </summary>
        Task<TSource> UpdateAsync(TSource entity);

        /// <summary>
        /// Delete entity.
        /// </summary>
        Task DeleteAsync(TSource entity);
    }
}
