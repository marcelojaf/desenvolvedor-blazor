using System.Linq.Expressions;
using VelozientComputers.Core.Common;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        #region Query Methods

        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns>Entity if found, null otherwise</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds entities based on a condition
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Collection of entities that match the condition</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks if any entity matches the specified condition
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        /// <param name="entities">Collection of entities to add</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Removes multiple entities
        /// </summary>
        /// <param name="entities">Collection of entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        #endregion
    }
}