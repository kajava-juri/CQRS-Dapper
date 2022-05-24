using CQRS.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRS.Models;
using CQRS.Services;

namespace UnitTests.Mocks
{
    public static class MockPlayersRepository
    {
        public static Mock<IPlayersService> GetPlayersService()
        {
            var players = new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Appearances = 20,
                    Goals = 56,
                    Name = "John Doe",
                    ShirtNo = 1
                },
                new Player
                {
                    Id = 2,
                    Appearances = 15,
                    Goals = 3,
                    Name = "Mark Rober",
                    ShirtNo = 2
                }
            };

            var mockService = new Mock<IPlayersService>();

            mockService.Setup(s => s.GetPlayersList()).ReturnsAsync(players);
            mockService.Setup(s => s.CreatePlayer(It.IsAny<Player>())).ReturnsAsync((Player player) =>
            {
                players.Add(player);
                return player;
            });
            mockService.Setup(s => s.UpdatePlayer(It.IsAny<Player>())).ReturnsAsync((Player player) =>
            {
                var playerToUpdate = players.First();
                playerToUpdate.Appearances = player.Appearances;
                playerToUpdate.Goals = player.Goals;
                return 1;
            });
            mockService.Setup(s => s.DeletePlayer(It.IsAny<Player>())).ReturnsAsync((Player player) =>
            {
                players.RemoveAt(0);
                return 1;
            });

            return mockService;
        }
    }
}
