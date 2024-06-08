using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Chess;
using new_chess_server.DTOs.GameMoveDTO;

namespace new_chess_server.Services.QuickPlay
{
    public class QuickPlayHandlerService : IQuickPlayHandlerService
    {
        private readonly StockfishService _stockfish;
        private readonly ObservableCollection<string> _output;

        public QuickPlayHandlerService()
        {
            _stockfish = new StockfishService();
            // this will contain all the output of the oracle
            _output = new ObservableCollection<string>();
            // in this way we redirect output to stdout of the main process
            _output.CollectionChanged += (sender, eventArgs) => Console.WriteLine(eventArgs.NewItems[0]);
            // in this way collect all the output
            _stockfish.DataReceived += (sender, eventArgs) => _output.Add(eventArgs.Data);

            _stockfish.Start();
        }
        public ServiceResponse<ResponseMoveDto> Move(RequestInputMoveDto requestInputMoveDto)
        {
            var response = new ServiceResponse<ResponseMoveDto>();
            var board = new ChessBoard();
            board = ChessBoard.LoadFromFen(requestInputMoveDto.Fen);

            // Check for endgame before asking Stockfish
            if (board.IsEndGame)
            {
                // Game ended - player (white) won
                response.Data = new ResponseMoveDto()
                {
                    IsGameOver = true,
                    WonSide = board.EndGame!.WonSide!.ToString(),
                    EndgameType = board.EndGame!.EndgameType.ToString(),
                };
            }
            else
            {
                // Ask Stockfish
                _stockfish.SendUciCommand("ucinewgame");
                _stockfish.SendUciCommand("isready");
                // Perform calculating
                _stockfish.SendUciCommand($"position fen {requestInputMoveDto.Fen}");
                _stockfish.SendUciCommand("go depth 10");

                _stockfish.Wait(500); // Wait a little

                while (!_output.Last().Contains("bestmove"))
                {
                    Console.WriteLine("Still calculating...");
                    _stockfish.Wait(500); // Wait a little
                }
                var bestMove = _output.Last();
                // Console.WriteLine(bestMove);

                // Make the black move
                board.Move(new Move(bestMove.Substring(9, 2), bestMove.Substring(11, 2)));
                // Check if AI (black) won
                if (board.IsEndGame)
                {
                    // Game ended - AI (black) won
                    response.Data = new ResponseMoveDto()
                    {
                        From = bestMove.Substring(9, 2),
                        To = bestMove.Substring(11, 2),
                        Promotion = bestMove.Length >= 14 ? bestMove.Substring(13, 1) : "",
                        IsGameOver = true,
                        WonSide = board.EndGame!.WonSide!.ToString(),
                        EndgameType = board.EndGame!.EndgameType.ToString(),
                    };
                }
                else
                {
                    // Game still going, so send the move back to the frontend
                    response.Data = new ResponseMoveDto()
                    {
                        From = bestMove.Substring(9, 2),
                        To = bestMove.Substring(11, 2),
                        Promotion = bestMove.Length >= 14 ? bestMove.Substring(13, 1) : ""
                    };
                }
            }

            return response;
        }
    }
}