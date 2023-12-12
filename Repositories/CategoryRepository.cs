using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<Category>> GetCategoryProducts(int id)
        {
            return await Get().Include(c => c.Products).Where(c => c.CategoryId == id).ToListAsync();
        }
    }
}
