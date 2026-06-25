using DistroFront.Http;
using DistroFront.Models;
using DistroFront.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace DistroFront.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistroApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiOptions>(configuration.GetSection(ApiOptions.SectionName));
        services.PostConfigure<ApiOptions>(options =>
        {
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                options.BaseUrl = "http://localhost:5130/";
            }
        });

        services.AddScoped<ILocalStorageService, LocalStorageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
        services.AddScoped<AuthHeaderHandler>();

        services.AddHttpClient<ApiClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<ApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute);
        }).AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IDistroService, DistroService>();
        services.AddScoped<IGenerateService, GenerateService>();

        return services;
    }
}
