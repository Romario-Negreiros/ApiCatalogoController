using ApiCatalogoController.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiCatalogoController.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext context;
        public Repository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task Add(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Get()
        {
            return context.Set<T>().AsNoTracking();
        }

        public async Task<T?> GetById(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.Set<T>().Update(entity);
        }
    }
}
