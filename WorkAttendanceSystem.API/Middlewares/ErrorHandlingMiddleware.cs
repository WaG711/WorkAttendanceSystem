using System.Net;
using System.Text.Json;
using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase) &&
                    context.Features.Get<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor>()?.ActionContext?.ModelState.IsValid == false)
                {
                    var errors = context.Features.Get<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor>()!
                        .ActionContext.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        code = "VALIDATION_ERROR",
                        message = "Validation failed",
                        details = errors
                    });

                    return;
                }

                await _next(context);
            }
            catch (DomainException dex)
            {
                await HandleExceptionAsync(context, dex.Code, dex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, "INTERNAL_ERROR", ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string code, string message, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                code,
                message
            });

            return context.Response.WriteAsync(result);
        }
    }
}
