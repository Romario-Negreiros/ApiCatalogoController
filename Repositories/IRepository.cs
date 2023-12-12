using System.Linq.Expressions;

namespace ApiCatalogoController.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task<T?> GetById(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
