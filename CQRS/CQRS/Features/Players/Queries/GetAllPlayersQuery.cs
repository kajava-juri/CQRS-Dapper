using CQRS.Data;
using CQRS.Models;
using CQRS.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Features.Players.Queries
{
    public class GetAllPlayersQuery : IRequest<List<Player>>
    {
        public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, List<Player>>
        {
            private readonly IPlayersService _playerService;

            public GetAllPlayersQueryHandler(IPlayersService playerService)
            {
                _playerService = playerService;
            }

            public async Task<List<Player>> Handle(GetAllPlayersQuery query, CancellationToken cancellationToken)
            {
                return await _playerService.GetPlayersList();
            }
        }
    }
}
