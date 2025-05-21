using DDDPlayground.Domain.Intefaces.Base;
using DDDPlayground.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayground.Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<T?> GetByIdAsync(Guid id)
             => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task AddAsync(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);
    }
}
