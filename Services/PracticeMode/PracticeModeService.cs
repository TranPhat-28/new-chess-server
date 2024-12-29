using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.Services.Stockfish;

namespace new_chess_server.Services.PracticeMode
{
    public class PracticeModeService : IPracticeModeService
    {
        private readonly IStockfishService _stockfish;
        private ChessBoard _board = new ChessBoard();

        public PracticeModeService(IStockfishService stockfishService)
        {
            _stockfish = stockfishService;
        }
        public async Task<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto)
        {
            var response = new ServiceResponse<ResponseMoveDto>();
            bool newGame = await StockfishStartNewGame();

            if (newGame == true)
            {
                // User has already made the move on Client so no need to call _board.Move() here
                _board = ChessBoard.LoadFromFen(requestInputMoveDto.Fen);
                if (_board.IsEndGame)
                {
                    // If this position is game over, user (WHITE) won, don't ask Stockfish and return
                    response.Data = new ResponseMoveDto()
                    {
                        IsGameOver = true,
                        WonSide = _board.EndGame!.WonSide!.ToString(),
                        EndgameType = _board.EndGame!.EndgameType.ToString(),
                    };
                }
                else
                {
                    // Game not over, ask Stockfish
                    string stockFishResponse = await _stockfish.GetStockfishMove(requestInputMoveDto.Fen);

                    // Make the black move
                    Move verboseMove = ParseMoveToVerbose(stockFishResponse, 'b');
                    _board.Move(verboseMove);

                    // Response data
                    var responseData = new ResponseMoveDto();
                    responseData.From = stockFishResponse.Substring(0, 2);
                    responseData.To = stockFishResponse.Substring(2, 2);
                    responseData.Promotion = stockFishResponse.Length == 5 ? stockFishResponse.Substring(4, 1) : "";

                    // Check if AI (BLACK) won
                    if (_board.IsEndGame)
                    {
                        // Game ended - AI (black) won
                        responseData.IsGameOver = true;
                        responseData.WonSide = _board.EndGame!.WonSide!.ToString();
                        responseData.EndgameType = _board.EndGame!.EndgameType.ToString();
                    }

                    response.Data = responseData;
                }
                return response;
            }
            else
            {
                throw new Exception("Cannot communicate with Stockfish");
            }
        }

        private async Task<bool> StockfishStartNewGame()
        {
            string output = await _stockfish.NewGame();
            if (output == "readyok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Move ParseMoveToVerbose(string move, char currentTurn)
        {
            if (string.IsNullOrEmpty(move))
                throw new ArgumentException("Move cannot be null or empty.");

            // Ensure move length is valid
            if (move.Length < 4 || move.Length > 5)
                throw new ArgumentException("Invalid move format.");

            // Extract start square, end square, and promotion (if present)
            string startSquare = move.Substring(0, 2);
            string endSquare = move.Substring(2, 2);
            string? promotion = move.Length == 5 ? move[4].ToString() : null;

            // Determine piece color
            string pieceColor = currentTurn == 'w' ? "w" : "b";

            // Build verbose format
            if (!string.IsNullOrEmpty(promotion))
            {
                string promotionPiece = pieceColor + promotion.ToLower(); // e.g., "wq" or "bn"
                return new Chess.Move($"{{{startSquare} - {endSquare} - {promotionPiece}}}");
            }
            else
            {
                return new Chess.Move($"{{{startSquare} - {endSquare}}}");
            }
        }
    }
}