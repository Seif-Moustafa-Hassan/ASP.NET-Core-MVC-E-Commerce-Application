using Microsoft.EntityFrameworkCore;
using ProjectData.Data;
using ProjectData.Models;
using ProjectServices.Services.Interfaces;

namespace ProjectServices.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<bool> AddToCartAsync(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
                return false;

            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Cart>> GetUserCartAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
