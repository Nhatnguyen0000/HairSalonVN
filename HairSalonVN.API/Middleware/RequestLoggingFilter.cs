using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HairSalonVN.API.Middleware
{
    public class RequestLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.ActionDescriptor.DisplayName;
            _logger.LogDebug("Executing {Action}", action);
            await next();
            _logger.LogDebug("Completed {Action}", action);
        }
    }
}
