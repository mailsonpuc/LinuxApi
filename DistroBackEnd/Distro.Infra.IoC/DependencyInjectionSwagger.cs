using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Distro.Infra.IoC
{
    public static class DependencyInjectionSwagger
    {
        public static IServiceCollection AddInfrastructureSwagger(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOpenApiDocument(options =>
            {
                // ===============================
                // INFO DA API
                // ===============================
                options.Title = "Distro API";
                options.Version = "v1";
                options.Description = "API para gerenciamento de distribuições Linux";

                // ===============================
                // JWT - SECURITY SCHEME
                // ===============================
                options.AddSecurity(
                    "JWT",
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Informe o token no formato: Bearer {seu_token}"
                    }
                );

                // ===============================
                // JWT - APLICA GLOBALMENTE
                // ===============================
                options.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT")
                );
            });

            return services;
        }
    }
}
