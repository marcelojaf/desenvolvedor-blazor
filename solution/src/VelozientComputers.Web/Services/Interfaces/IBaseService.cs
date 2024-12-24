namespace VelozientComputers.Web.Services.Interfaces;

/// <summary>
/// Base interface for all HTTP service operations
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TCreateDTO">DTO type for creation</typeparam>
/// <typeparam name="TUpdateDTO">DTO type for updates</typeparam>
public interface IBaseService<TEntity, TCreateDTO, TUpdateDTO>
{
    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns>Collection of entities</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets an entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity if found</returns>
    Task<TEntity> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="createDto">DTO with creation data</param>
    /// <returns>Created entity</returns>
    Task<TEntity> CreateAsync(TCreateDTO createDto);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="updateDto">DTO with update data</param>
    /// <returns>Updated entity</returns>
    Task<TEntity> UpdateAsync(int id, TUpdateDTO updateDto);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    Task DeleteAsync(int id);
}