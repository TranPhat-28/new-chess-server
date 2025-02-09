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
        public async Task<ServiceResponse<GameRoomDto>> GetRoomInfoById(string roomId)
        {
            var response = new ServiceResponse<GameRoomDto>();
            var gameRoomInfo = await _gameLobbyTracker.GetRoomInfoById(roomId);

            if (gameRoomInfo is null)
            {
                response.Data = null;
                response.Message = "Room does not exist";
            }
            else
            {
                response.Data = _mapper.Map<GameRoomDto>(gameRoomInfo);
            }
            return response;
        }
    }
}