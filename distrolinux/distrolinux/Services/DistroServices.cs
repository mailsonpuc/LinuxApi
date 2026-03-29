
using System.Text.Json;
using distrolinux.Components.Pages;

namespace distrolinux.Services;

public class DistroServices
{

    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "http://localhost:5130/api/Distro";

    public DistroServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<Distro>> GetDistrosAsync()
    {
        var response = await _httpClient.GetAsync(ApiBaseUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Distro>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

        public async Task<Distro> GetDistroByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Distro>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        





}
