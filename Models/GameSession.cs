using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class GameSession
    {
        public string Id { get; set; } = "";
        public int HostId { get; set; }
        public int PlayerId { get; set; }
        public int MovingPlayerId { get; set; }
        public List<string> History { get; set; } = new List<string>();
        public bool IsHostChecked { get; set; }
        public bool IsPlayerChecked { get; set; }
        public bool IsGameOver { get; set; }
        public int WinnerId { get; set; }
        public string WinReason { get; set; } = "";
    }
}