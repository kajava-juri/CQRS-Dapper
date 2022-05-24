using CQRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        private readonly FootballDbContext _context;
        public PlayerRepository(FootballDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
