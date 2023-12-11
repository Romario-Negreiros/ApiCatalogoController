using ApiCatalogoController.Context;
using ApiCatalogoController.Models;

namespace ApiCatalogoController.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public IEnumerable<Product> GetProductsByPrice(int minRange, int maxRange)
        {
            return Get().Where(p => p.Price >= minRange && p.Price <= maxRange).ToList();
        }
    }
}
