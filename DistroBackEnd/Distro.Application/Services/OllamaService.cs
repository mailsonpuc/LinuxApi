using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Distro.Application.Interfaces;

namespace Distro.Application.Services
{
    public class OllamaService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaUrl;

        public OllamaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _ollamaUrl = configuration["Ollama:Url"] ?? "http://localhost:11434/api/generate";
        }

        public async Task<string> GenerateResponseAsync(string model, string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model,
                    prompt,
                    stream = false
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_ollamaUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro ao chamar a API do Ollama: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("response", out var responseProperty))
                    {
                        return responseProperty.GetString() ?? "Sem resposta";
                    }
                }

                return "Resposta inválida da API";
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão com a API do Ollama: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar resposta da IA: {ex.Message}", ex);
            }
        }
    }
}
