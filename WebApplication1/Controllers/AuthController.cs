using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectData.Models;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View();
            }

            var user = new ApplicationUser
            {
                Email = email,
                UserName = name // IMPORTANT: use email as username
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // 🔥 SHOW REAL ERRORS (THIS IS THE FIX)
            ViewBag.Error = string.Join(" | ",
                result.Errors.Select(e => e.Description));

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
        public async Task<IActionResult> Login(string email, string password)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Product");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                password,
                false,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Product");

            ViewBag.Error = "Invalid login attempt";
            return View();
        }

        // ================= LOGOUT =================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}