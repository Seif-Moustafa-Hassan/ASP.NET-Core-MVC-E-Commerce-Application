using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Authorization;
using ProjectData.Models;
using ProjectServices.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        // =========================
        // VIEW ADD TO CART FORM
        // =========================
        [Permission("Add to Cart")]
        [HttpGet]
        public async Task<IActionResult> AddToCartForm(int productId)
        {
            var product = await _cartService.GetProductByIdAsync(productId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // ADD TO CART (POST)
        // =========================
        [Permission("Add to Cart")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var success = await _cartService.AddToCartAsync(user.Id, productId, quantity);

            if (!success)
            {
                TempData["Error"] = "Invalid quantity selected.";
                return RedirectToAction("Index", "Product");
            }

            TempData["Success"] = "Product added to cart successfully!";
            return RedirectToAction("Index", "Product");
        }

        // =========================
        // VIEW MY CART
        // =========================
        [Permission("View Cart")]
        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var cartItems = await _cartService.GetUserCartAsync(user.Id);

            return View(cartItems);
        }
    }
}