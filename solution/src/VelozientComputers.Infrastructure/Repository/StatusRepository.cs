using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Infrastructure.Data;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Implementation of the computer status repository
    /// </summary>
    public class StatusRepository : Repository<ComputerStatus>, IStatusRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StatusRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public StatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<ComputerStatus> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.LocalizedName.ToLower() == name.ToLower());
        }

        #endregion
    }
}