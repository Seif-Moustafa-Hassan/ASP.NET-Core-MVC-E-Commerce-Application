using ProjectData.Models;

namespace ProjectServices.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Create(Product product);
        void Update(Product product);
        bool Delete(int id);
    }
}
