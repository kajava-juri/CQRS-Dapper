using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore
{
    public interface IDapperWrapper
    {
        IEnumerable<T> Query<T>(IDbConnection connection, string sql);
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql);
        Task<T> QuerySingleAsync<T>(IDbConnection connection, string query, DynamicParameters parameters);
        Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string query, object param);
        Task ExecuteAsync(IDbConnection connection, string query, object param);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string procedureName, DynamicParameters parameters, CommandType command);
        Task<Dapper.SqlMapper.GridReader> QueryMultipleAsync<T>(IDbConnection connection, string query, object param);
        Task<T> ReadSingleOrDefaultAsync<T>(Dapper.SqlMapper.GridReader multi);
        Task<IEnumerable<T>> ReadAsync<T>(SqlMapper.GridReader multi);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(IDbConnection connection, string query, Func<TFirst, TSecond, TReturn> map);
        Task<int> ExecuteAsync(IDbConnection connection, string query, DynamicParameters parameters, IDbTransaction transaction);
    }
}
