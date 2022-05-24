using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data.Repositories
{
    public interface IBaseRepository<T>
    {
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(int id);
        void Delete(T entity);
    }
}
