using System.Linq.Expressions;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.API.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SalonDbContext _ctx;
        private readonly DbSet<T> _set;

        public Repository(SalonDbContext ctx)
        { _ctx = ctx; _set = ctx.Set<T>(); }

        public async Task<T?> GetByIdAsync(Guid id)
            => await _set.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _set.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate)
            => await _set.AsNoTracking().Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _set.AsNoTracking();
            foreach (var inc in includes) q = q.Include(inc);
            return await q.Where(predicate).ToListAsync();
        }

        public Task<IEnumerable<T>> FindWithTrackingAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _set;
            foreach (var inc in includes) q = q.Include(inc);
            return Task.FromResult(q.Where(predicate).ToList().AsEnumerable());
        }

        public void Remove(T entity) => _set.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _set.AnyAsync(predicate);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            => await _set.CountAsync(predicate);

        public async Task AddAsync(T entity) => await _set.AddAsync(entity);
        public void Update(T entity) => _set.Update(entity);
        public void Delete(T entity) => _set.Remove(entity);
        public async Task<int> SaveChangesAsync() => await _ctx.SaveChangesAsync();
    }
}
