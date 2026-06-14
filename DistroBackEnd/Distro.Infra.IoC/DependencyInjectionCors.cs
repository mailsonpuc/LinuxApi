using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distro.Infra.IoC
{
    public static class DependencyInjectionCors
    {
        public static IServiceCollection AddInfrastructureCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Política global que permite qualquer origem, método e cabeçalho.
            // Caso deseje restringir, carregue valores de "configuration" e ajuste aqui.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
    }
}
