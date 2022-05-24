using Dapper;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore
{
    public class DapperWrapper : IDapperWrapper
    {
        public Task ExecuteAsync(IDbConnection connection, string query, object param)
        {
            return connection.ExecuteAsync(query, param);
        }

        public Task<int> ExecuteAsync(IDbConnection connection, string query, DynamicParameters parameters, IDbTransaction transaction)
        {
            return connection.ExecuteAsync(query, parameters, transaction: transaction);
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string sql)
        {
            return connection.Query<T>(sql);
        }
        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql)
        {
            return connection.QueryAsync<T>(sql);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(IDbConnection connection, string query, Func<TFirst, TSecond, TReturn> map)
        {
            return connection.QueryAsync<TFirst, TSecond, TReturn>(query, map);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string procedureName, DynamicParameters parameters, CommandType command)
        {
            return connection.QueryFirstOrDefaultAsync<T>(procedureName, parameters, commandType: command);
        }

        public Task<Dapper.SqlMapper.GridReader> QueryMultipleAsync<T>(IDbConnection connection, string query, object param)
        {
            return connection.QueryMultipleAsync(query, param);
        }

        public Task<T> QuerySingleAsync<T>(IDbConnection connection, string query, DynamicParameters parameters)
        {
            return connection.QuerySingleAsync<T>(query, parameters);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string query, object param)
        {
            return connection.QuerySingleOrDefaultAsync<T>(query, param);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(SqlMapper.GridReader multi)
        {
            return multi.ReadAsync<T>();
        }

        public Task<T> ReadSingleOrDefaultAsync<T>(SqlMapper.GridReader multi)
        {
            return multi.ReadSingleOrDefaultAsync<T>();
        }
    }
}
