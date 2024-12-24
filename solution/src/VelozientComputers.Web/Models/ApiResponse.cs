using System.Net;

namespace VelozientComputers.Web.Models;

/// <summary>
/// Generic API response with data
/// </summary>
/// <typeparam name="T">Type of the response data</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets the response data
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response message
    /// </summary>
    public string Message { get; set; }
}

/// <summary>
/// Basic API response without data
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Gets or sets the HTTP status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response message
    /// </summary>
    public string Message { get; set; }
}