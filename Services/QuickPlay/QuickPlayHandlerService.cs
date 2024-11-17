using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private ChessBoard _board;

        public QuickPlayHandlerService()
        {
            _stockfish = new StockfishService();
            _board = new ChessBoard();

            // This will contain all the output
            _output = new ObservableCollection<string>();
            // Collect all the output to Stockfish
            _stockfish.DataReceived += (sender, eventArgs) => _output.Add(eventArgs.Data);
            // Start Stockfish and send "uci" command to initiate
            InitiateStockfish(_stockfish, _output);
        }

        private static async void InitiateStockfish(StockfishService stockfish, ObservableCollection<string> output)
        {
            // Create a TaskCompletionSource to wait for "uciok"
            var tcs = new TaskCompletionSource();

            // Define the temporary handler
            NotifyCollectionChangedEventHandler handler = null;
            handler = (sender, eventArgs) =>
            {
                if (eventArgs.NewItems is not null)
                {
                    foreach (var item in eventArgs.NewItems)
                    {
                        string? outputLine = item?.ToString();

                        if (outputLine is not null)
                        {
                            // Check for "uciok"
                            if (outputLine.Trim() == "uciok")
                            {
                                // Detach the handler once "uciok" is found
                                output.CollectionChanged -= handler;

                                // Complete the TaskCompletionSource
                                tcs.TrySetResult();
                            }
                        }
                    }
                }

            };

            // Attach the temporary handler
            output.CollectionChanged += handler;

            // Start Stockfish and send the "uci" command
            stockfish.Start();
            stockfish.SendUciCommand("uci");

            // Wait asynchronously for "uciok"
            await tcs.Task;

            Console.WriteLine("Stockfish is initialized");
        }

        public async Task<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto)
        {
            var response = new ServiceResponse<ResponseMoveDto>();
            _board = ChessBoard.LoadFromFen(requestInputMoveDto.Fen);

            // Check for endgame before asking Stockfish
            if (_board.IsEndGame)
            {
                // Game ended - player (white) won
                response.Data = new ResponseMoveDto()
                {
                    IsGameOver = true,
                    WonSide = _board.EndGame!.WonSide!.ToString(),
                    EndgameType = _board.EndGame!.EndgameType.ToString(),
                };
            }
            else
            {
                // Ask Stockfish
                // GetStockfishReady(_stockfish, _output);
                // Perform calculating
                //await GetBestMove(_stockfish, _output, requestInputMoveDto.Fen, 5);

                /*
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
                */
            }

            return response;
        }

        private static void GetStockfishReady(StockfishService stockfish, ObservableCollection<string> output)
        {

            stockfish.SendUciCommand("ucinewgame");
            stockfish.SendUciCommand("isready");

            if (output.Last().Contains("readyok"))
            {
                return;
            }
            else
            {
                throw new Exception("Cannot start Stockfish");
            }
        }

        private static async Task<string> GetBestMove(StockfishService stockfish, ObservableCollection<string> output, string fen, int depth)
        {
            stockfish.SendUciCommand($"position fen {fen}");
            stockfish.SendUciCommand($"go depth {depth}");

            // Wait for output to reach length of depth
            await WaitForCollectionLengthAsync(output, depth);

            string bestMove = output.LastOrDefault();
            return bestMove;
        }

        private static Task WaitForCollectionLengthAsync(ObservableCollection<string> collection, int targetLength)
        {
            var tcs = new TaskCompletionSource();

            NotifyCollectionChangedEventHandler handler = null;
            handler = (sender, args) =>
            {
                if (collection.Count >= targetLength)
                {
                    // Remove the event handler to prevent memory leaks
                    collection.CollectionChanged -= handler;

                    // Mark the task as complete
                    tcs.TrySetResult();
                }
            };

            // Attach the event handler
            collection.CollectionChanged += handler;

            // Check the condition immediately in case it's already met
            if (collection.Count >= targetLength)
            {
                collection.CollectionChanged -= handler;
                tcs.TrySetResult();
            }

            return tcs.Task;
        }
    }
}