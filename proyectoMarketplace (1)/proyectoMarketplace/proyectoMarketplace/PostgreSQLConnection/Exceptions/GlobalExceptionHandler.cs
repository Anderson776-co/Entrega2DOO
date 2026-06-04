using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace APIDazma.Infrastructure.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, errorPhrase) = exception switch
            {
                UnauthorizedException ex => (ex.StatusCode, "Unauthorized"),
                ValidationException ex => (ex.StatusCode, "Bad Request"),
                ForbiddenException ex => (ex.StatusCode, "Forbidden"),
                ConflictException ex => (ex.StatusCode, "Conflict"),
                InternalServerException ex => (ex.StatusCode, "Internal Server Error"),
                AppException ex => (ex.StatusCode, "App Error"),
                _ => (500, "Internal Server Error")
            };

            var message = exception is AppException
                ? exception.Message
                : "Ha ocurrido un error interno.";

            if (exception is not AppException)
                _logger.LogError(exception, "Unhandled exception");

            var response = new ErrorResponse
            {
                Status = statusCode,
                Error = errorPhrase,
                Message = message
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}