using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS
{
    public class DapperWrapper : IDapperWrapper
    {
        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql)
        {
            return connection.QueryAsync<T>(sql);
        }
    }
}
