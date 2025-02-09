using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.MultiplayerModeDTO;

namespace new_chess_server.Services.Multiplayer
{
    public interface IMultiplayerService
    {
        Task<ServiceResponse<GameRoomDto>> GetRoomInfoById(string roomId);
    }
}