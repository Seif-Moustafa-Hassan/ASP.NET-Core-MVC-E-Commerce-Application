using ProjectData.Models;

namespace ProjectServices.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(
            string name,
            string email,
            string password,
            string confirmPassword);

        Task<bool> LoginAsync(string email, string password);

        Task LogoutAsync();
    }
}