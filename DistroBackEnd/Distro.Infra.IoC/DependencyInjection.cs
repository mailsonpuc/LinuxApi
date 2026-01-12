using Distro.Application.Interfaces;
using Distro.Application.Mappings;
using Distro.Application.Services;
using Distro.Domain.Interfaces;
using Distro.Infra.Data.Context;
using Distro.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distro.Infra.IoC
{
    /// <summary>
    /// Classe responsável por centralizar todas as injeções de dependência
    /// da camada de Infraestrutura (IoC Container).
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Método de extensão que registra os serviços de infraestrutura,
        /// repositórios, serviços de aplicação, AutoMapper e DbContext.
        /// </summary>
        /// <param name="services">Container de serviços do ASP.NET</param>
        /// <param name="configuration">Configurações da aplicação (appsettings.json)</param>
        /// <returns>ServiceCollection com as dependências registradas</returns>
        public static IServiceCollection AddInfrastructureIoC(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ===============================
            // CONFIGURAÇÃO DO EF CORE / DB
            // ===============================
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    // String de conexão vinda do appsettings.json
                    configuration.GetConnectionString("DefaultConnection"),

                    // Define onde ficarão as migrations
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            // ===============================
            // REGISTRO DOS REPOSITÓRIOS
            // ===============================
            // Repositórios são responsáveis pelo acesso aos dados
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDistroRepository, DistroRepository>();

            // ===============================
            // REGISTRO DOS SERVICES (Application Layer)
            // ===============================
            // Services encapsulam regras de negócio e orquestram o domínio
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDistroService, DistroService>();

            // ===============================
            // CONFIGURAÇÃO DO AUTOMAPPER
            // ===============================
            // Responsável por mapear Entidades <-> DTOs
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
