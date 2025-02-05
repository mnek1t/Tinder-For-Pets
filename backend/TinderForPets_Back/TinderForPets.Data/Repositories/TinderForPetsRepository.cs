using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class TinderForPetsRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly TinderForPetsDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public TinderForPetsRepository(TinderForPetsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async virtual Task<Guid> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async virtual Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async virtual Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
