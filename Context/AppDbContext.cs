using ApiCatalogoController.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Product> Products  { get; set; } = default!;
    }
}
