using Distro.API.Models;
using Distro.Domain.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Distro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixedwindow")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authenticate;
        private readonly IConfiguration _configuration;

        public TokenController(
            IAuthenticate authenticate,
            IConfiguration configuration)
        {
            _authenticate = authenticate;
            _configuration = configuration;
        }

        // ===============================
        // REGISTER
        // ===============================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModels model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errorMessage) = await _authenticate.RegisterUser(
                model.Email!,
                model.Password!
            );

            if (!success)
            {
                if (!string.IsNullOrEmpty(errorMessage) && errorMessage.Contains("e-mail ja registrado"))
                    return BadRequest("e-mail ja registrado");

                return BadRequest(errorMessage ?? "Erro ao registrar usuário");
            }

            return Ok("Usuário registrado com sucesso");
        }

        // ===============================
        // LOGIN
        // ===============================
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModels model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authenticated = await _authenticate.Authenticate(
                model.Email!,
                model.Password!
            );

            if (!authenticated)
                return Unauthorized("Usuário ou senha inválidos");

            return GenerateToken(model.Email!);
        }

        // ===============================
        // JWT TOKEN
        // ===============================
        private UserToken GenerateToken(string email)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(
                double.Parse(jwtSettings["ExpireMinutes"]!)
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
