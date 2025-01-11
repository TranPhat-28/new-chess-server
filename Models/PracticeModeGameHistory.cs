using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class PracticeModeGameHistory
    {
        public int Id { get; set; }
        public List<MoveHistoryItem> Moves { get; set; } = new List<MoveHistoryItem>();
        // Require User
        public User User { get; set; } = null!;
        public int UserId { get; set; }
    }
}