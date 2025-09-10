﻿using System.Diagnostics;

namespace GenericRepo_Dapper.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            _logger.LogInformation("Request: {method} {path}", request.Method, request.Path);

            await _next(context); // Chuyển tới middleware tiếp theo

            stopwatch.Stop();
            _logger.LogInformation("Response: {statusCode} ({duration}ms)", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
