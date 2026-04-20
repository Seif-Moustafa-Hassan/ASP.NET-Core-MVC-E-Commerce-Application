using Microsoft.AspNetCore.Mvc;
using ProjectServices.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string name,
            string email,
            string password,
            string confirmPassword)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            var (success, errors) = await _authService.RegisterAsync(
                name, email, password, confirmPassword);

            if (success)
            {
                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Login");
            }

            TempData["Error"] = string.Join(" | ", errors);
            return View();
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            var success = await _authService.LoginAsync(email, password);

            if (success)
                return RedirectToAction("Index", "Product");

            TempData["Error"] = "Invalid login attempt";
            return View();
        }

        // ================= LOGOUT =================

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}