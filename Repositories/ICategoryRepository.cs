using ApiCatalogoController.Models;
using ApiCatalogoController.Pagination;

namespace ApiCatalogoController.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetCategories(PaginationParameters paginationParameters);
        Task<Category?> GetCategoryProducts(int id);
    }
}
