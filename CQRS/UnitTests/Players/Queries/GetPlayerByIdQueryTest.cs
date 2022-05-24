using CQRS.Features.Players.Queries;
using CQRS.Models;
using CQRS.Services;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;
using static CQRS.Features.Players.Queries.GetPlayerByIdQuery;

namespace UnitTests.Players.Queries
{
    public class GetPlayerByIdQueryTest
    {
        private readonly Mock<IPlayersService> _mockRepo;
        public GetPlayerByIdQueryTest()
        {
            _mockRepo = MockPlayersRepository.GetPlayersService();
        }

        [Fact]
        public async Task GetPlayerByIdQuery_ReturnPlayer()
        {
            var handler = new GetPlayerByIdQueryHandler(_mockRepo.Object);

            var result = await handler.Handle(new GetPlayerByIdQuery(), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<Player>();
        }
    }
}
