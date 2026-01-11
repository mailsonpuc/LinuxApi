using Distro.Domain.Interfaces;
using Distro.Infra.Data.Context;
using Distro.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distro.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIoC(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDistroRepository, DistroRepository>();

            return services;
        }
    }
}
