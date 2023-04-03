using Serilog;
using Serilog.Events;

namespace LearnSmartCoding.EssentialProducts.API.Common
{
    public static class ConfigureRequestLogging
    {
        public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                // health check exclusion inspiration: 
                // https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-excluding-health-check-endpoints-from-serilog-request-logging/
                options.GetLevel = ExcludeHealthChecks;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
                    // could add user information: var user = httpContext.User.Identity;                    
                };
            });

        }

        private static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            var userAgent = ctx.Request.Headers["User-Agent"].FirstOrDefault() ?? "";
            return ctx.Request.Path.Value.EndsWith("health", StringComparison.CurrentCultureIgnoreCase) ||
                   userAgent.Contains("HealthCheck", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
