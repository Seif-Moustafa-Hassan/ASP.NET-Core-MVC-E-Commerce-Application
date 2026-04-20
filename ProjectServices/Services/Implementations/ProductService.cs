using ProjectData.Data;
using ProjectData.Models;
using ProjectServices.Services.Interfaces;


namespace ProjectServices.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public void Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }
    }
}
