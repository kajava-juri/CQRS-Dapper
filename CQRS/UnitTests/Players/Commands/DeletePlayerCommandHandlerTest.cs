using CQRS.Features.Players.Commands;
using CQRS.Models;
using CQRS.Services;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;
using Shouldly;
using static CQRS.Features.Players.Commands.DeletePlayerCommand;

namespace UnitTests.Players.Commands
{
    public class DeletePlayerCommandHandlerTest
    {
        private readonly Mock<IPlayersService> _mockRepo;
        public DeletePlayerCommandHandlerTest()
        {
            _mockRepo = MockPlayersRepository.GetPlayersService();
        }

        [Fact]
        public async Task DeletePlayer_DeletesPlayer()
        {
            var handler = new DeletePlayerCommandHandler(_mockRepo.Object);

            var result = await handler.Handle(new DeletePlayerCommand() { Id = 1 }, CancellationToken.None);

            var players = await _mockRepo.Object.GetPlayersList();

            players.Count.ShouldBe(1);
        }
    }
}
