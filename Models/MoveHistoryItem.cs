using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class MoveHistoryItem
    {
        public int Id { get; set; }
        public int MoveIndex { get; set; }
        public string Side { get; set; } = string.Empty;
        public string Move { get; set; } = string.Empty;
        // Required one-to-many for cascade delete
        public int PracticeModeGameHistoryId { get; set; }
        public PracticeModeGameHistory practiceModeGameHistory { get; set; } = null!;
    }
}