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
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Get()
        {
            return context.Set<T>().AsNoTracking();
        }

        public T? GetById(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().SingleOrDefault(predicate);
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.Set<T>().Update(entity);
        }
    }
}
