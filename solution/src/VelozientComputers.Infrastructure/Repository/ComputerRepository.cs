using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Infrastructure.Data;

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
        /// <param name="context">Database context</param>
        public ComputerRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<Computer> GetWithCurrentAssignmentAsync(int id)
        {
            return await _dbSet
                .Include(c => c.UserAssignments.Where(a => a.AssignEndDate == null))
                    .ThenInclude(a => a.User)
                .Include(c => c.StatusAssignments.OrderByDescending(s => s.AssignDate).Take(1))
                    .ThenInclude(s => s.Status)
                .Include(c => c.Manufacturer)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllWithCurrentAssignmentsAsync()
        {
            return await _dbSet
                .Include(c => c.UserAssignments.Where(a => a.AssignEndDate == null))
                    .ThenInclude(a => a.User)
                .Include(c => c.StatusAssignments.OrderByDescending(s => s.AssignDate).Take(1))
                    .ThenInclude(s => s.Status)
                .Include(c => c.Manufacturer)
                .ToListAsync();
        }

        #endregion
    }
}