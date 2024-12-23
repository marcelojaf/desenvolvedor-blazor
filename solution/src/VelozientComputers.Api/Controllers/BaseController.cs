using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Base controller that provides common functionality for all API controllers
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ApiResponse<T>(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ObjectResult(new ApiResponse<T>
            {
                Data = data,
                StatusCode = statusCode,
                Success = IsSuccessStatusCode(statusCode)
            })
            {
                StatusCode = (int)statusCode
            };
        }

        protected IActionResult ApiResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ObjectResult(new ApiResponse
            {
                StatusCode = statusCode,
                Success = IsSuccessStatusCode(statusCode)
            })
            {
                StatusCode = (int)statusCode
            };
        }

        private bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}