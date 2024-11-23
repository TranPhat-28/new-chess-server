using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Chess;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.Services.Stockfish;

namespace new_chess_server.Services.QuickPlay
{
    public class QuickPlayHandlerService : IQuickPlayHandlerService
    {
        private readonly IStockfishService _stockfish;
        private ChessBoard _board = new ChessBoard();

        public QuickPlayHandlerService(IStockfishService stockfish)
        {
            _stockfish = stockfish;
        }

        public async Task<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto)
        {
            var response = new ServiceResponse<ResponseMoveDto>();
            string output = await _stockfish.NewGame();
            return response;
        }

        public async Task<string> Test()
        {
            string output = await _stockfish.NewGame();
            return output;
        }
    }
}