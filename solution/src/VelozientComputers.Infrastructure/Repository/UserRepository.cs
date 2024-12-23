using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Infrastructure.Data;

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
        /// <param name="context">Database context</param>
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<User> GetWithCurrentComputersAsync(int id)
        {
            return await _dbSet
                .Include(u => u.ComputerAssignments.Where(a => a.AssignEndDate == null))
                    .ThenInclude(a => a.Computer)
                        .ThenInclude(c => c.Manufacturer)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllWithCurrentComputersAsync()
        {
            return await _dbSet
                .Include(u => u.ComputerAssignments.Where(a => a.AssignEndDate == null))
                    .ThenInclude(a => a.Computer)
                        .ThenInclude(c => c.Manufacturer)
                .ToListAsync();
        }

        #endregion
    }
}