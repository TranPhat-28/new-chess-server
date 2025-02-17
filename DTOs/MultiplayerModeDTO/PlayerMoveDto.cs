using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.MultiplayerModeDTO
{
    public class PlayerMoveDto
    {
        public string RoomId { get; set; } = "";
        public string Move { get; set; } = "";
    }
}