using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static CQRS.Features.Players.Queries.GetAllPlayersQuery;
using CQRS.Services;
using CQRS.Features.Players.Queries;
using System.Threading;
using CQRS.Models;
using Shouldly;
using UnitTests.Mocks;

namespace UnitTests.Players.Queries
{
    public class GetAllPlayersQueryTest
    {
        private readonly Mock<IPlayersService> _mockRepo;
        public GetAllPlayersQueryTest()
        {
            _mockRepo = MockPlayersRepository.GetPlayersService();
        }

        [Fact]
        public async Task GetAllPlayers_ReturnListOfPlayers()
        {
            var handler = new GetAllPlayersQueryHandler(_mockRepo.Object);

            var result = await handler.Handle(new GetAllPlayersQuery(), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Player>>();
            result.Count.ShouldBe(2);
        }
    }
}
