using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.LobbyDTO
{
    public class GameLobbyDto
    {
        public string Id { get; set; } = "";
        public string HostName { get; set; } = "";
        public int HostId { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool IsPlaying { get; set; } = false;
        public bool IsFull { get; set; } = false;
    }
}