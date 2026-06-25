using System.ComponentModel.DataAnnotations;

namespace DistroFront.DTOs;

public sealed class GenerateRequestDto
{
    [Required(ErrorMessage = "Informe o modelo.")]
    public string? Model { get; set; } = "qwen2.5:3b";

    [Required(ErrorMessage = "Informe uma pergunta sobre Linux.")]
    public string? Prompt { get; set; }
}

public sealed class GenerateResponseDto
{
    public string Answer { get; set; } = string.Empty;
}
