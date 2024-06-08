using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.GameMoveDTO
{
    public class RequestInputMoveDto
    {
        public string Fen { get; set; } = "";
    }
}