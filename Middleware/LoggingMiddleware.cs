using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog;
using Serilog.Context;

namespace WebApplication1.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var endpoint = context.GetEndpoint();
            var controllerName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
            var actionMethodname = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName;
            var userName = context.Request.Headers["username"].FirstOrDefault() ??
                           context.User.Identity?.Name ?? "Anonymus";
            var machinename = Environment.MachineName;

            var requestID = Activity.Current?.Id ?? context.TraceIdentifier;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown user";

            using (LogContext.PushProperty("Controller", controllerName))
            using (LogContext.PushProperty("Action", actionMethodname))
            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("Machine", machinename))
            using (LogContext.PushProperty("RequestId", requestID))
            using (LogContext.PushProperty("IPAddress", ipAddress))
            {
                Log.Information("API call started");
                await _next(context); // Continue pipeline
                Log.Information("API call ended");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An exception occurred while processing the request");
            throw; // Rethrow the exception
        }

    }
}