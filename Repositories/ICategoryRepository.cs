using ApiCatalogoController.Models;

namespace ApiCatalogoController.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryProducts(int id);
    }
}
