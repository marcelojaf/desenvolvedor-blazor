using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Implementation of the User repository
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UserRepository class
        /// </summary>
        /// <param name="context">Entity Framework context</param>
        public UserRepository(DbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<User> GetWithCurrentComputersAsync(int id)
        {
            return await _dbSet
                .Include(u => u.ComputerAssignments)
                    .ThenInclude(a => a.Computer)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllWithCurrentComputersAsync()
        {
            return await _dbSet
                .Include(u => u.ComputerAssignments)
                    .ThenInclude(a => a.Computer)
                .ToListAsync();
        }

        #endregion
    }
}