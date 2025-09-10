using Microsoft.AspNetCore.Mvc;

namespace GenericRepo_Dapper.Common
{
    public class ResponseResult<T> : IActionResult
    {
        private readonly ApiResponse<T> _response;
        private readonly int _statusCode;

        public ResponseResult(ApiResponse<T> response, int statusCode = StatusCodes.Status200OK) 
        {
            _response = response;
            _statusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_response);
            var response = context.HttpContext.Response;
            response.StatusCode = _statusCode;
            response.ContentType = "application/json";
            await response.WriteAsync(json);
        }
    }
}
