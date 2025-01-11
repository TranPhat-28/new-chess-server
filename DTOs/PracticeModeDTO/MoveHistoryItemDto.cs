using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.PracticeModeDTO
{
    public class MoveHistoryItemDto
    {
        public int MoveIndex { get; set; }
        public string Side { get; set; } = string.Empty;
        public string Move { get; set; } = string.Empty;
    }
}