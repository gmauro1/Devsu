using Devsu.Entities;
using Devsu.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Devsu.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MovimientoContext _context;
        private readonly DbSet<T> _entities;

        public Repository(MovimientoContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T?> GetById(int id, string? include = null, Expression<Func<T, bool>> filter = null)
        {
            var query = _entities.AsQueryable();
            if (include == null)
            {
                return await _entities.FindAsync(id);
            }
            else
            {
                query = query.Include(include);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(List<string>? includes = null, Expression<Func<T, bool>> filter = null)
        {
            var query = _entities.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
             _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        { 
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
