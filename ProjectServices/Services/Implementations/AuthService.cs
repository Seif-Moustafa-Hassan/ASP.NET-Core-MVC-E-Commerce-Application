using Microsoft.AspNetCore.Identity;
using ProjectData.Models;
using ProjectServices.Services.Interfaces;

namespace ProjectServices.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(
            string name,
            string email,
            string password,
            string confirmPassword)
        {
            if (password != confirmPassword)
                return (false, new[] { "Passwords do not match" });

            var user = new ApplicationUser
            {
                Email = email,
                UserName = name
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                return (true, Enumerable.Empty<string>());

            return (false, result.Errors.Select(e => e.Description));
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                password,
                false,
                false
            );

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}