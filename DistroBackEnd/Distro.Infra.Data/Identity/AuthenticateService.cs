using System;
using System.Linq;
using System.Threading.Tasks;
using Distro.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace Distro.Infra.Data.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }


        public async Task<(bool Success, string? ErrorMessage)> RegisterUser(string email, string password)
        {
            // Verifica se já existe um usuário com o e-mail
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                return (false, "e-mail ja registrado");
            }

            var AppUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(AppUser, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(AppUser, isPersistent: false);
                return (true, null);
            }

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return (false, errors);
        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}