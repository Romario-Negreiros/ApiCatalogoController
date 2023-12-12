using ApiCatalogoController.Models;

namespace ApiCatalogoController.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetCategoryProducts(int id);
    }
}
