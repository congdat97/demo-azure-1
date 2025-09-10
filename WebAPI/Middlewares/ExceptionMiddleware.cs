using System.Net;
using System.Text.Json;

namespace GenericRepo_Dapper.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // tiếp tục pipeline

                if (!context.Response.HasStarted && (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600))
                {
                    await HandleStatusCodeAsync(context, context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleStatusCodeAsync(HttpContext context, int statusCode)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "Bad Request",
                    StatusCodes.Status401Unauthorized => "Unauthorized",
                    StatusCodes.Status403Forbidden => "Forbidden",
                    StatusCodes.Status404NotFound => "Not Found",
                    _ => "Error"
                },
                data = (object?)null
            };

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                message = "Internal Server Error",
                detail = ex.Message, // Có thể tắt khi chạy production
                data = (object?)null
            };

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}
