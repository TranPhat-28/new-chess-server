using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.PracticeModeDTO
{
    public class UpdateGameHistoryDto
    {
        public List<MoveHistoryItemDto> Moves { get; set; } = new List<MoveHistoryItemDto>();
    }
}