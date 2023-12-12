using ApiCatalogoController.Context;

namespace ApiCatalogoController.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProductRepository? productRepository;
        private CategoryRepository? categoryRepository;
        public AppDbContext context;
        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
        }
        public IProductRepository ProductRepository
        {
            get
            {
                return productRepository = productRepository ?? new ProductRepository(context);
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                return categoryRepository = categoryRepository ?? new CategoryRepository(context);
            }
        }
        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
        public async Task Dispose()
        {
            await context.DisposeAsync();
        }
    }
}
