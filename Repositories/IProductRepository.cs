using ApiCatalogoController.Models;

namespace ApiCatalogoController.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByPrice(int minRange, int maxRange);
    }
}
