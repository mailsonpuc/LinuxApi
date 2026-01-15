using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System.Reflection;

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
                // Configurações básicas de identificação
                options.Title = "Distro API";
                options.Version = "v1";

                // ===============================
                // CARREGAR DOCUMENTAÇÃO XML
                // ===============================
                // Isso permite que o NSwag leia os sumários (summary) dos Controllers
                var assembly = Assembly.GetExecutingAssembly();
                // Geralmente o XML fica na API, então buscamos o nome do assembly da API
                // Se esta classe estiver em um projeto diferente, garanta que o XML da API seja copiado
                // ou aponte diretamente para o nome do arquivo da API.
                options.SchemaSettings.GenerateXmlObjects = true;

                // Adicionando informações detalhadas via PostProcess
                options.PostProcess = document =>
                {
                    document.Info.Description = "API para gerenciamento de distribuições Linux";

                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Github Contact",
                        Url = "https://github.com/mailsonpuc",
                        Email = "mailson.costa@protonmail.com"
                    };

                    document.Info.License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = "https://example.com/license"
                    };
                };

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

                // Aplica a segurança JWT globalmente para todos os endpoints
                options.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT")
                );
            });

            return services;
        }
    }
}