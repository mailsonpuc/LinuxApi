using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distro.Infra.IoC
{
    public static class DependencyInjectionRateLimiter
    {
        public static IServiceCollection AddInfrastructureRateLimiter(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
                {
                    options.PermitLimit = 100;  // apenas 100 requests permitidos a cada
                    options.Window = TimeSpan.FromSeconds(5); // 5 segundos
                    options.QueueLimit = 0;
                });

                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }
    }
}
