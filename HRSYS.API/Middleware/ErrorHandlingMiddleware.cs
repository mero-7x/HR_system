using System.Net;
using System.Security.Authentication;
using HRSYS.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HRSYS.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden && !context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = 403,
                        message = " Access denied: Only HR members can access this endpoint."
                    });
                }
            }
            catch (AccountDisabledException ex)
            {
                _logger.LogWarning($"Account disabled: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(new { status = 403, message = ex.Message });
            }
            catch (NotFoundException ex)
            {

                _logger.LogWarning($"NotFound: {ex.Message}");
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (HRSYS.Application.Exceptions.AuthenticationException ex)
            {

                _logger.LogWarning($"Authentication failed: {ex.Message}");
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }

            catch (UnauthorizedAccessException ex)
            {

                _logger.LogWarning($"Unauthorized access: {ex.Message}");
                await HandleExceptionAsync(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (ValidationException ex)
            {

                _logger.LogInformation($"Validation failed: {ex.Message}");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = (int)statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
