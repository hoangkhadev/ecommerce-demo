using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Ecommerce.Api.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            // Logic phân loại lỗi của bạn
            var (statusCode, title) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid Operation"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            httpContext.Response.StatusCode = statusCode;

            var response = new
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Errors = (exception as ValidationException)?.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
