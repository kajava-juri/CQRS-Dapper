using CQRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Services
{
    public interface IPlayersService
    {
        Task<List<Player>> GetPlayersList();
        Task<Player> GetPlayerById(int id);
        Task<Player> CreatePlayer(Player player);
        Task<int> UpdatePlayer(Player player);
        Task<int> DeletePlayer(Player player);
    }
}
