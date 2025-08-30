using System.Net;
using System.Text.Json;
using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest &&
                    context.Items.ContainsKey("ModelStateErrors") &&
                    context.Items["ModelStateErrors"] is Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
                {
                    var errors = modelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    await WriteErrorResponseAsync(context, "VALIDATION_ERROR", "Validation failed", errors);
                }
            }
            catch (DomainException dex)
            {
                await WriteErrorResponseAsync(context, dex.Code, dex.Message, null, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await WriteErrorResponseAsync(context, "INTERNAL_ERROR", ex.Message, null, HttpStatusCode.InternalServerError);
            }
        }

        private static Task WriteErrorResponseAsync(HttpContext context, string code, string message, object? details = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = new
            {
                code,
                message,
                details
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
            return context.Response.WriteAsync(json);
        }
    }
}
