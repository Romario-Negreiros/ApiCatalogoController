using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByPrice(int minRange, int maxRange)
        {
            return await Get().Where(p => p.Price >= minRange && p.Price <= maxRange).ToListAsync();
        }
    }
}
