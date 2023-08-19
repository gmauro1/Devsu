using System.Linq.Expressions;

namespace Devsu.Repository.Interface
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll(List<string>? includes = null, Expression<Func<T, bool>> filter = null);
        Task<T?> GetById(int id, string? include = null, Expression<Func<T, bool>> filter = null);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
