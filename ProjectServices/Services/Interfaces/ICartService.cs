using ProjectData.Models;

namespace ProjectServices.Services.Interfaces
{
    public interface ICartService
    {
        Task<Product?> GetProductByIdAsync(int productId);

        Task<bool> AddToCartAsync(string userId, int productId, int quantity);

        Task<List<Cart>> GetUserCartAsync(string userId);
    }
}
