using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess;
using new_chess_server.Data;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.DTOs.PracticeModeDTO;
using new_chess_server.Services.Stockfish;

namespace new_chess_server.Services.PracticeMode
{
    public class PracticeModeService : IPracticeModeService
    {
        private readonly IStockfishService _stockfish;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private ChessBoard _board = new ChessBoard();

        public PracticeModeService(IStockfishService stockfishService, DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _stockfish = stockfishService;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> CheckIfSavedGameExist()
        {
            var response = new ServiceResponse<bool>();

            int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var previousGame = await _dataContext.PracticeModeGameHistories
                .Include(game => game.Moves)
                .FirstOrDefaultAsync(game => game.UserId == userId);

            if (previousGame is null)
            {
                throw new Exception("Cannot load your data now");
            }
            else
            {
                if (previousGame.Moves.Count == 0)
                {
                    response.Data = false;
                    response.Message = "No saved game found";
                }
                else
                {
                    response.Data = true;
                    response.Message = "Saved game found";
                }
            }
            return response;
        }

        public async Task<ServiceResponse<List<MoveHistoryItem>>> GetSavedGameHistory()
        {
            var response = new ServiceResponse<List<MoveHistoryItem>>();

            int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var savedGame = await _dataContext.PracticeModeGameHistories.FirstOrDefaultAsync(game => game.UserId == userId);

            if (savedGame is null)
            {
                throw new Exception("Csnnot load saved game");
            }
            else
            {
                response.Data = savedGame.Moves;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteSavedGameHistory()
        {
            var response = new ServiceResponse<int>();

            int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var savedGame = await _dataContext.PracticeModeGameHistories.Include(game => game.Moves).FirstOrDefaultAsync(game => game.UserId == userId);

            if (savedGame is null)
            {
                throw new Exception("Cannot delete saved game");
            }
            else
            {
                _dataContext.MoveHistoryItems.RemoveRange(savedGame.Moves);
                await _dataContext.SaveChangesAsync();

                response.Data = savedGame.Id;
                response.Message = "Previously saved game has been remove";
                return response;
            }
        }

        public async Task<ServiceResponse<int>> UpdateSavedGameHistory(UpdateGameHistoryDto updateGameHistoryDto)
        {
            var response = new ServiceResponse<int>();

            int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var gameHistory = await _dataContext.PracticeModeGameHistories.FirstOrDefaultAsync(history => history.UserId == userId);

            if (gameHistory is null)
            {
                throw new Exception("Cannot find your data");
            }

            // Map to model
            var newMovesList = _mapper.Map<List<MoveHistoryItemDto>, List<MoveHistoryItem>>(updateGameHistoryDto.Moves);
            foreach (var move in newMovesList)
            {
                move.PracticeModeGameHistoryId = gameHistory.Id; // Set the foreign key
                move.practiceModeGameHistory = gameHistory;      // Set navigation property (optional)
            }

            // Insert all moves            
            gameHistory.Moves = newMovesList;

            await _dataContext.SaveChangesAsync();

            response.Data = gameHistory.Id;
            response.Message = "Your game has been saved";

            return response;
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