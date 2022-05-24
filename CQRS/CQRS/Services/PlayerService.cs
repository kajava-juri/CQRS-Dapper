using CQRS.Data;
using CQRS.Data.Repositories;
using CQRS.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPlayerRepository _repository;
        private readonly IDapperContext _dapperContext;
        private readonly IDapperWrapper _dapperWrapper;

        public PlayersService(IUnitOfWork context, IDapperContext dapperContext, IDapperWrapper dapperWrapper)
        {
            _uow = context;
            _repository = context.Players;
            _dapperContext = dapperContext;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<List<Player>> GetPlayersList()
        {
            //return await _context.Players
            //    .ToListAsync();

            string query = "SELECT * FROM Players";

            using (var connection = _dapperContext.CreateConnection())
            {
                var companies = await _dapperWrapper.QueryAsync<Player>(connection, query);
                return companies.ToList();
            }
        }

        public async Task<Player> GetPlayerById(int id)
        {
            //return await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var query = "SELECT * FROM Players WHERE Id = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Player>(query, new { id });

                return company;
            }
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await _repository.Create(player);
            await _uow.CompleteAsync();
            return player;

            //var query = "INSERT INTO Players (ShirtNo, Name, Appearances, Goals) VALUES (@ShirtNo, @Name, @Appearances, @Goals)" +
            //    "SELECT CAST(SCOPE_IDENTITY() as int)";

            //var parameters = new DynamicParameters();
            //parameters.Add("ShirtNo", player.ShirtNo, DbType.Int32);
            //parameters.Add("Name", player.Name, DbType.String);
            //parameters.Add("Appearances", player.Appearances, DbType.Int32);
            //parameters.Add("Goals", player.Goals, DbType.Int32);

            //using (var connection = _dapperContext.CreateConnection())
            //{

            //    await connection.ExecuteAsync(query, parameters);
            //}
            //return player;

        }

        public async Task<int> UpdatePlayer(Player player)
        {
            await _repository.Update(player);
            return await _uow.CompleteAsync();
        }

        public async Task<int> DeletePlayer(Player player)
        {
            _repository.Delete(player);
            return await _uow.CompleteAsync();
        }
    }
}
