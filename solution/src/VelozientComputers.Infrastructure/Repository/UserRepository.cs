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
            var user = await _dbSet
                .Include(u => u.ComputerAssignments)
                    .ThenInclude(a => a.Computer)
                        .ThenInclude(c => c.Manufacturer)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                // Filter current assignments in memory
                user.ComputerAssignments = user.ComputerAssignments
                    .Where(a => a.AssignEndDate == null)
                    .ToList();
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllWithCurrentComputersAsync()
        {
            var users = await _dbSet
                .Include(u => u.ComputerAssignments)
                    .ThenInclude(a => a.Computer)
                        .ThenInclude(c => c.Manufacturer)
                .ToListAsync();

            // Filter assignments in memory for SQLite compatibility
            foreach (var user in users)
            {
                user.ComputerAssignments = user.ComputerAssignments
                    .Where(a => a.AssignEndDate == null)
                    .ToList();
            }

            return users;
        }

        #endregion
    }
}