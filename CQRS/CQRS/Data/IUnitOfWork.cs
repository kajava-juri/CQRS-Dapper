using CQRS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data
{
    public interface IUnitOfWork
    {
        public IPlayerRepository Players { get; }
        Task<int> CompleteAsync();
    }
}
