using System.Threading.Tasks;

namespace Distro.Application.Interfaces
{
    public interface IAIService
    {
        Task<string> GenerateResponseAsync(string model, string prompt);
    }
}
