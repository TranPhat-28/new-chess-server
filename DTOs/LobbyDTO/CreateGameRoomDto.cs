using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.LobbyDTO
{
    public class CreateGameRoomDto
    {
        public bool IsPublicRoom { get; set; }
        public string RoomPassword { get; set; } = "";
    }
}