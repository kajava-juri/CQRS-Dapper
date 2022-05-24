using CQRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data.Repositories
{
    public interface IPlayerRepository : IBaseRepository<Player>
    {
    }
}
