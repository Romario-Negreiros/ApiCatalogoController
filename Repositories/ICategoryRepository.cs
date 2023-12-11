using ApiCatalogoController.Models;

namespace ApiCatalogoController.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoryProducts(int id);
    }
}
