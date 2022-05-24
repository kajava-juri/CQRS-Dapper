using CQRS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPlayerRepository Players { get; private set; }
        private readonly FootballDbContext _context;
        public UnitOfWork(FootballDbContext context, IPlayerRepository playerRepository)
        {
            _context = context;
            Players = playerRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
