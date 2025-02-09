using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.MultiplayerModeDTO
{
    public class GameRoomDto
    {
        public string Id { get; set; } = "";
        public RoomPlayer Host { get; set; } = null!;
        public RoomPlayer? Player { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool IsPlaying { get; set; } = false;
    }
}