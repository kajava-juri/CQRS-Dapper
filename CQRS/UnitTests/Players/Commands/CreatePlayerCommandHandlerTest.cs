using CQRS.Features.Players.Commands;
using CQRS.Models;
using CQRS.Services;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;
using Shouldly;
using static CQRS.Features.Players.Commands.CreatePlayerCommand;

namespace UnitTests.Players.Commands
{
    public class CreatePlayerCommandHandlerTest
    {
        private readonly Mock<IPlayersService> _mockRepo;
        private readonly PlayerForCreationDto _playerDto;
        public CreatePlayerCommandHandlerTest()
        {
            _mockRepo = MockPlayersRepository.GetPlayersService();

            _playerDto = new PlayerForCreationDto
            {
                Appearances = 21,
                Name = "John Doe",
                Goals = 27,
                ShirtNo = 11
            };
        }

        [Fact]
        public async Task CreatePlayer_CreatesPlayer()
        {
            var handler = new CreatePlayerCommandHandler(_mockRepo.Object);

            var result = await handler.Handle(new CreatePlayerCommand() { Name = _playerDto.Name, Goals = _playerDto.Goals, Appearances = _playerDto.Appearances, ShirtNo = _playerDto.ShirtNo }, CancellationToken.None);

            var players = await _mockRepo.Object.GetPlayersList();

            result.ShouldBeOfType<Player>();
            players.Count.ShouldBe(3);
        }
    }
}
