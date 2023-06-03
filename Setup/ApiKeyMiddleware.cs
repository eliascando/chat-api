using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace chatApi.Setup
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IOptions<ApiKeyConfiguration> apiToken)
        {
            if(context.Request.Headers.TryGetValue("apiKey", out StringValues apiKey))
            {
                if (apiKey == apiToken.Value.Value)
                    await _next(context);
                else
                    ReturnApyKeyNotFound();
            }
            else
            {
                ReturnApyKeyNotFound();
            }
            void ReturnApyKeyNotFound()
            {
                throw new UnauthorizedAccessException(message: "The API Key is missing");
            }
        }
    }
}