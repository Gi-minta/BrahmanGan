using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BrahmanGan.API.Filters;

/// <summary>
/// Filtro para logging de acciones del controlador
/// </summary>
public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation(
            "Executing action: {ActionName} on controller: {ControllerName}",
            context.ActionDescriptor.DisplayName,
            context.Controller.GetType().Name);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation(
            "Executed action: {ActionName} with status code: {StatusCode}",
            context.ActionDescriptor.DisplayName,
            context.HttpContext.Response.StatusCode);
    }
}
