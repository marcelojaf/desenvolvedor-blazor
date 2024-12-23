using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Infrastructure.Data;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Implementation of the computer manufacturer repository
    /// </summary>
    public class ManufacturerRepository : Repository<ComputerManufacturer>, IManufacturerRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ManufacturerRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<ComputerManufacturer> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());
        }

        #endregion
    }
}