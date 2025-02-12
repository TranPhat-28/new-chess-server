using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.MultiplayerModeDTO;
using new_chess_server.SignalR;

namespace new_chess_server.Services.Multiplayer
{
    public class MultiplayerService : IMultiplayerService
    {
        private readonly GameLobbyTracker _gameLobbyTracker;
        private readonly IMapper _mapper;

        public MultiplayerService(GameLobbyTracker gameLobbyTracker, IMapper mapper)
        {
            _gameLobbyTracker = gameLobbyTracker;
            _mapper = mapper;
        }
        public ServiceResponse<string> GetNewRoomId()
        {
            var response = new ServiceResponse<string>();
            response.Data = Guid.NewGuid().ToString("N").Substring(0, 8); // 8-character unique ID

            return response;
        }
    }
}