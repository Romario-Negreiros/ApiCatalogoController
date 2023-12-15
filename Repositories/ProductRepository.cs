using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
using ApiCatalogoController.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<PagedList<Product>> GetProducts(PaginationParameters paginationParameters)
        {
            return await PagedList<Product>.ToPagedList(Get().OrderBy(on => on.Name), paginationParameters.PageNumber, paginationParameters.PageSize);
        }
        public async Task<List<Product>> GetProductsByPrice(int minRange, int maxRange)
        {
            return await Get().Where(p => p.Price >= minRange && p.Price <= maxRange).ToListAsync();
        }
    }
}
