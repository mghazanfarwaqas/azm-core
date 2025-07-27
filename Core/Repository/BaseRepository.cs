using Core.Data;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Interface;

namespace Core.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : EntityBase
    {
        private readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public virtual async Task<T> Add(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        protected abstract Task<IEnumerable<T>> GetEntityAll(DbContext context);
        protected abstract Task<T> GetEntityById(DbContext context, int id);
        protected abstract Task<IEnumerable<T>> GetEntitySearch(DbContext context, Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes);
        protected abstract Task<PagedList<T>> GetPagedEntityList(DbContext context, Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes, int page, int pageSize);

        public virtual async Task<T> Update(T entity)
        {
            _context.Entry(entity).Property(x => x.CreatedById).IsModified = false;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await (GetEntityAll(_context));
        }

        public async Task<T> GetById(int id)
        {
            return await GetEntityById(_context, id);
        }

        public async Task Remove(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remove(int id)
        {
            T entity = await GetEntityById(_context, id);
            if (entity != null)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> SearchEntity(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes)
        {
            if (includes == null) includes = new List<Expression<Func<T, object>>>();
            return await GetEntitySearch(_context, where, includes);
        }

        public async Task<PagedList<T>> GetPagedEntity(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> includes, int page, int pageSize)
        {
            if (includes == null) includes = new List<Expression<Func<T, object>>>();
            return await GetPagedEntityList(_context, where, includes, page, pageSize);
        }
    }
}
