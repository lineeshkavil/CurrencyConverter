using CurrencyConverter.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Services.Repository
{
    public class Repository<TEntity> where TEntity : class
    {
        private readonly CurrencyDbContext _context;

        public Repository(CurrencyDbContext context)
        {
            _context = context;
        }
        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
       
        public async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
                _context.Set<TEntity>().Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

}
