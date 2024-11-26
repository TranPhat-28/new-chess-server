using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.GameMoveDTO;

namespace new_chess_server.Services.Stockfish
{
    public interface IStockfishService
    {
        Task<string> NewGame();
        Task<string> GetStockfishMove(string fen);
    }
}