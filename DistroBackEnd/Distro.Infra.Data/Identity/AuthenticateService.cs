using System;
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



        public async Task<bool> RegisterUser(string email, string password)
        {
            var AppUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };
            var result = await _userManager.CreateAsync(AppUser, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(AppUser, isPersistent: false);
            }

            return result.Succeeded;
        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}