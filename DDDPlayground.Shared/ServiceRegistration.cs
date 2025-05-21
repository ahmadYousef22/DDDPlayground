using DDDPlayground.Shared.Middleware;
using Microsoft.AspNetCore.Builder;

namespace DDDPlayground.Shared
{
    public static class ServiceRegistration
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
