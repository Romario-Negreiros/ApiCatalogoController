using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
using ApiCatalogoController.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<PagedList<Category>> GetCategories(PaginationParameters paginationParameters)
        {
            return await PagedList<Category>.ToPagedList(Get().OrderBy(on => on.Name), paginationParameters.PageNumber, paginationParameters.PageSize);
        }
        public async Task<Category?> GetCategoryProducts(int id)
        {
            return await Get().Include(c => c.Products).Where(c => c.CategoryId == id).FirstOrDefaultAsync();
        }
    }
}
