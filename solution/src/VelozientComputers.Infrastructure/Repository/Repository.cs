using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Generic repository implementation for data access operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Entity Framework context
        /// </summary>
        protected readonly DbContext _context;

        /// <summary>
        /// DbSet for the entity
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Repository class
        /// </summary>
        /// <param name="context">Entity Framework context</param>
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <inheritdoc/>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <inheritdoc/>
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc/>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #endregion
    }
}