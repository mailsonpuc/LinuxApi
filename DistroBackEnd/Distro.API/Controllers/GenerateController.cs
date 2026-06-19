using System;
using System.Text.RegularExpressions;
using Distro.API.DTOs;
using Distro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Distro.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableRateLimiting("generate")]
    public class GenerateController : ControllerBase
    {
        private readonly IAIService _aiService;

        public GenerateController(IAIService aiService)
        {
            _aiService = aiService;
        }


        /// <summary>
        /// Gerar resposta pelo modelo local qwen2.5:3b, sobre distro linux.
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<GenerateResponseDTO>> Generate([FromBody] GenerateRequestDTO request)
        {
            if (request == null)
                return BadRequest("Requisição inválida.");

            if (string.IsNullOrWhiteSpace(request.Model))
                return BadRequest("O campo 'model' é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("O campo 'prompt' é obrigatório.");

            var prompt = request.Prompt.Trim();
            if (!Regex.IsMatch(prompt, "\\blinux\\b", RegexOptions.IgnoreCase))
                return BadRequest("A pergunta deve ser sobre Linux.");

            try
            {
                var answer = await _aiService.GenerateResponseAsync(request.Model, prompt);
                return Ok(new GenerateResponseDTO { Answer = answer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar resposta: {ex.Message}");
            }
        }
    }
}
