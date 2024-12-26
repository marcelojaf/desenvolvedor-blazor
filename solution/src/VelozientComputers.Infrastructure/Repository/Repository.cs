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
            await SaveChangesAsync();

            if (entity.Id == 0)
            {
                var entry = _context.Entry(entity);
                entry.CurrentValues.SetValues(await _dbSet.FindAsync(entry.Property("Id").CurrentValue));
            }
        }

        /// <inheritdoc/>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await SaveChangesAsync();

            foreach (var entity in entities)
            {
                if (entity.Id == 0)
                {
                    var entry = _context.Entry(entity);
                    entry.CurrentValues.SetValues(await _dbSet.FindAsync(entry.Property("Id").CurrentValue));
                }
            }
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await SaveChangesAsync();
        }

        #endregion

        #region SaveChanges

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of entities written to the database</returns>
        protected async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _context?.Dispose();
        }

        #endregion
    }
}