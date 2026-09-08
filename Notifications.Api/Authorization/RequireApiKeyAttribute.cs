using Microsoft.AspNetCore.Mvc.Filters;

namespace Notifications.Api.Authorization
{
    public class RequireApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "X-Api-Key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var expectedApiKey = config["InternalApiKey"];

            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult(new { error = "API Key is missing." });
                return;
            }

            if (!string.Equals(extractedApiKey, expectedApiKey, StringComparison.Ordinal))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult(new { error = "Invalid API Key." });
                return;
            }

            await next();
        }
    }
}
