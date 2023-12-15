using ApiCatalogoController.Models;
using ApiCatalogoController.Pagination;

namespace ApiCatalogoController.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetProducts(PaginationParameters paginationParameters);
        Task<List<Product>> GetProductsByPrice(int minRange, int maxRange);
    }
}
