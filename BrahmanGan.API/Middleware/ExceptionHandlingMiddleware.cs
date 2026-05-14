using System.Net;
using System.Text.Json;
using BrahmanGan.Application.Exceptions;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.API.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            BrahmanGan.Domain.Exceptions.EntityNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            BrahmanGan.Application.Exceptions.EntityNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            BusinessRuleException => (HttpStatusCode.BadRequest, exception.Message),
            DomainException => (HttpStatusCode.BadRequest, exception.Message),
            FluentValidation.ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))
            ),
            Application.Exceptions.ValidationException appValidationEx => (
                HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(appValidationEx.Errors)
            ),
            Application.Exceptions.ApplicationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An internal error occurred.")
        };

        response.StatusCode = (int)statusCode;

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
        }

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode = (int)statusCode
        });

        return response.WriteAsync(result);
    }
}
