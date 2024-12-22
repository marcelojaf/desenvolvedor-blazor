using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Implementation of the Computer repository
    /// </summary>
    public class ComputerRepository : Repository<Computer>, IComputerRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ComputerRepository class
        /// </summary>
        /// <param name="context">Entity Framework context</param>
        public ComputerRepository(DbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<Computer> GetWithCurrentAssignmentAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Assignments)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllWithCurrentAssignmentsAsync()
        {
            return await _dbSet
                .Include(c => c.Assignments)
                    .ThenInclude(a => a.User)
                .ToListAsync();
        }

        #endregion
    }
}