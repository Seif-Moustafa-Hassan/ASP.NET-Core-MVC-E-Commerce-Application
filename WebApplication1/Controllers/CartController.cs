using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authorization;
//using WebApplication1.Data;
using ProjectData.Data;
using ProjectData.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // VIEW ADD TO CART FORM
        // =========================
        [Permission("Add to Cart")]
        [HttpGet]
        public async Task<IActionResult> AddToCartForm(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

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

            if (quantity <= 0)
            {
                TempData["Error"] = "Invalid quantity selected.";
                return RedirectToAction("Index", "Product");
            }

            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c =>
                    c.UserId == user.Id &&
                    c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();

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

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            return View(cartItems);
        }
    }
}