using System.Linq.Expressions;
using Core.Utils;

namespace Core.Interface
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Remove(T entity);
        Task Remove(int id);
        Task<IEnumerable<T>> SearchEntity(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes);
        Task<PagedList<T>> GetPagedEntity(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes, int page, int pageSize);
    }
}
