using CQRS.Features.Players.Commands;
using CQRS.Models;
using CQRS.Services;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using static CQRS.Features.Players.Commands.UpdatePlayerCommand;
using System.Linq;

namespace UnitTests.Players.Commands
{
    public class UpdatePlayerCommandHandlerTest
    {
        private readonly Mock<IPlayersService> _mockRepo;
        private readonly Player _updatedPlayer;
        public UpdatePlayerCommandHandlerTest()
        {
            _mockRepo = MockPlayersRepository.GetPlayersService();

            _updatedPlayer = new Player
            {
                Id = 1,
                Appearances = 21,
                Goals = 70,
                Name = "John Doe",
                ShirtNo = 1
            };
        }

        [Fact]
        public async Task UpdatePlayer_UpdatesPlayer()
        {
            var handler = new UpdatePlayerCommandHandler(_mockRepo.Object);
            int id = _updatedPlayer.Id;
            _mockRepo.Setup(s => s.GetPlayerById(id)).ReturnsAsync(_updatedPlayer);

            var result = await handler.Handle(new UpdatePlayerCommand() { Id = _updatedPlayer.Id, Name = _updatedPlayer.Name, Goals = _updatedPlayer.Goals, Appearances = _updatedPlayer.Appearances, ShirtNo = _updatedPlayer.ShirtNo }, CancellationToken.None);

            var updatedPlayer = _mockRepo.Object.GetPlayersList().Result.First();

            Assert.NotNull(updatedPlayer);
            Assert.IsType<Player>(updatedPlayer);
            Assert.Equal(_updatedPlayer.Appearances, updatedPlayer.Appearances);
        }

    }
}
