using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly FootballDbContext _context;
        public BaseRepository(FootballDbContext context)
        {
            _context = context;
        }
        public async Task Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public async Task Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
    }
}
