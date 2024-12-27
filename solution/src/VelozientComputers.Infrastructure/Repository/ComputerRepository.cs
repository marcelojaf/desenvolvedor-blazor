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
        /// <summary>
        /// Initializes a new instance of the ComputerRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public ComputerRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<Computer> GetWithCurrentAssignmentAsync(int id)
        {
            // Load computer with all related data
            var computer = await _dbSet
                .Include(c => c.Manufacturer)
                .Include(c => c.UserAssignments)
                    .ThenInclude(a => a.User)
                .Include(c => c.StatusAssignments)
                    .ThenInclude(s => s.Status)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (computer != null)
            {
                // Filter to only current user assignments in memory
                computer.UserAssignments = computer.UserAssignments
                    .Where(a => a.AssignEndDate == null)
                    .ToList();

                // Take only the most recent status assignment in memory
                computer.StatusAssignments = computer.StatusAssignments
                    .OrderByDescending(s => s.AssignDate)
                    .Take(1)
                    .ToList();
            }

            return computer;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllWithCurrentAssignmentsAsync()
        {
            // Load computers with all related data
            var computers = await _dbSet
                .Include(c => c.Manufacturer)
                .Include(c => c.UserAssignments)
                    .ThenInclude(a => a.User)
                .Include(c => c.StatusAssignments)
                    .ThenInclude(s => s.Status)
                .ToListAsync();

            // Process the data in memory
            foreach (var computer in computers)
            {
                // Filter to only current user assignments
                computer.UserAssignments = computer.UserAssignments
                    .Where(a => a.AssignEndDate == null)
                    .ToList();

                // Take only the most recent status assignment
                computer.StatusAssignments = computer.StatusAssignments
                    .OrderByDescending(s => s.AssignDate)
                    .Take(1)
                    .ToList();
            }

            return computers;
        }

        /// <inheritdoc/>
        public override async Task RemoveAsync(Computer entity)
        {
            var computer = await _dbSet
                .Include(c => c.StatusAssignments)
                .Include(c => c.UserAssignments)
                .FirstOrDefaultAsync(c => c.Id == entity.Id);

            if (computer != null)
            {
                _dbSet.Remove(computer);
                await SaveChangesAsync();
            }
        }
    }
}