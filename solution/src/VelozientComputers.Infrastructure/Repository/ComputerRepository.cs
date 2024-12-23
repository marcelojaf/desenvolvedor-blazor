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
            var computers = await (
                from c in _dbSet
                where c.Id == id
                select new Computer
                {
                    Id = c.Id,
                    ComputerManufacturerId = c.ComputerManufacturerId,
                    SerialNumber = c.SerialNumber,
                    Specifications = c.Specifications,
                    ImageUrl = c.ImageUrl,
                    PurchaseDate = c.PurchaseDate,
                    WarrantyExpirationDate = c.WarrantyExpirationDate,
                    CreateDate = c.CreateDate,
                    Manufacturer = new ComputerManufacturer
                    {
                        Id = c.Manufacturer.Id,
                        Name = c.Manufacturer.Name,
                        SerialRegex = c.Manufacturer.SerialRegex,
                        CreateDate = c.Manufacturer.CreateDate
                    },
                    UserAssignments = c.UserAssignments
                        .Where(a => a.AssignEndDate == null)
                        .Select(a => new ComputerUserAssignment
                        {
                            Id = a.Id,
                            ComputerId = a.ComputerId,
                            UserId = a.UserId,
                            AssignStartDate = a.AssignStartDate,
                            AssignEndDate = a.AssignEndDate,
                            User = new User
                            {
                                Id = a.User.Id,
                                FirstName = a.User.FirstName,
                                LastName = a.User.LastName,
                                EmailAddress = a.User.EmailAddress,
                                CreateDate = a.User.CreateDate
                            }
                        }).ToList(),
                    StatusAssignments = c.StatusAssignments
                        .OrderByDescending(s => s.AssignDate)
                        .Take(1)
                        .Select(s => new ComputerStatusAssignment
                        {
                            Id = s.Id,
                            ComputerId = s.ComputerId,
                            ComputerStatusId = s.ComputerStatusId,
                            AssignDate = s.AssignDate,
                            Status = new ComputerStatus
                            {
                                Id = s.Status.Id,
                                LocalizedName = s.Status.LocalizedName,
                                CreateDate = s.Status.CreateDate
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();

            return computers;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllWithCurrentAssignmentsAsync()
        {
            var computers = await (
                from c in _dbSet
                select new Computer
                {
                    Id = c.Id,
                    ComputerManufacturerId = c.ComputerManufacturerId,
                    SerialNumber = c.SerialNumber,
                    Specifications = c.Specifications,
                    ImageUrl = c.ImageUrl,
                    PurchaseDate = c.PurchaseDate,
                    WarrantyExpirationDate = c.WarrantyExpirationDate,
                    CreateDate = c.CreateDate,
                    Manufacturer = new ComputerManufacturer
                    {
                        Id = c.Manufacturer.Id,
                        Name = c.Manufacturer.Name,
                        SerialRegex = c.Manufacturer.SerialRegex,
                        CreateDate = c.Manufacturer.CreateDate
                    },
                    UserAssignments = c.UserAssignments
                        .Where(a => a.AssignEndDate == null)
                        .Select(a => new ComputerUserAssignment
                        {
                            Id = a.Id,
                            ComputerId = a.ComputerId,
                            UserId = a.UserId,
                            AssignStartDate = a.AssignStartDate,
                            AssignEndDate = a.AssignEndDate,
                            User = new User
                            {
                                Id = a.User.Id,
                                FirstName = a.User.FirstName,
                                LastName = a.User.LastName,
                                EmailAddress = a.User.EmailAddress,
                                CreateDate = a.User.CreateDate
                            }
                        }).ToList(),
                    StatusAssignments = c.StatusAssignments
                        .OrderByDescending(s => s.AssignDate)
                        .Take(1)
                        .Select(s => new ComputerStatusAssignment
                        {
                            Id = s.Id,
                            ComputerId = s.ComputerId,
                            ComputerStatusId = s.ComputerStatusId,
                            AssignDate = s.AssignDate,
                            Status = new ComputerStatus
                            {
                                Id = s.Status.Id,
                                LocalizedName = s.Status.LocalizedName,
                                CreateDate = s.Status.CreateDate
                            }
                        }).ToList()
                }).ToListAsync();

            return computers;
        }

        #endregion
    }
}