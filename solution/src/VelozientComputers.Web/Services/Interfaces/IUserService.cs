using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Web.Services.Interfaces;

/// <summary>
/// Interface for user-related HTTP operations
/// </summary>
public interface IUserService : IBaseService<UserDTO, CreateUserDTO, UpdateUserDTO>
{
    /// <summary>
    /// Gets a user by their email address
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <returns>User if found</returns>
    Task<UserDTO> GetByEmailAsync(string email);
}