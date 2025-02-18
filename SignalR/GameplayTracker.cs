using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess;

namespace new_chess_server.SignalR
{
    public class GameplayTracker
    {
        private static readonly List<GameSession> GameSessionList = new List<GameSession>();
        public Task StartGameplay(string roomId, int hostId, int playerId)
        {
            var newGame = new GameSession
            {
                Id = roomId,
                HostId = hostId,
                PlayerId = playerId,
                MovingPlayerId = hostId
            };

            lock (GameSessionList)
            {
                GameSessionList.Add(newGame);
            }

            return Task.CompletedTask;
        }

        public Task<int> GetMovingPlayerId(string roomId)
        {
            lock (GameSessionList)
            {
                var id = GameSessionList.FirstOrDefault(r => r.Id == roomId)?.MovingPlayerId;
                return Task.FromResult(id ?? -1);
            }
        }

        public Task<GameSession> MakeMove(string roomId, string move, string playerId)
        {
            lock (GameSessionList)
            {
                var room = GameSessionList.FirstOrDefault(r => r.Id == roomId);
                if (room is null)
                {
                    throw new Exception($"[GameplayTracker] Cannot find game room with id {roomId}");
                }
                // Save new move
                room.History.Add(move);
                // Change move turn to the other player
                if (room.MovingPlayerId == room.HostId)
                {
                    room.MovingPlayerId = room.PlayerId;
                }
                else
                {
                    room.MovingPlayerId = room.HostId;
                }

                // Verify king checked status
                var board = new ChessBoard() { AutoEndgameRules = AutoEndgameRules.All };
                room.History.ForEach(move =>
                {
                    Move verboseMove = ParseMoveToVerbose(move, playerId == room.HostId.ToString() ? 'w' : 'b');
                    board.Move(verboseMove);
                });

                // Reset the checkmate status
                room.IsHostChecked = false;
                room.IsPlayerChecked = false;
                
                if (board.WhiteKingChecked)
                {
                    room.IsHostChecked = true;
                }
                if (board.BlackKingChecked)
                {
                    room.IsPlayerChecked = true;
                }

                return Task.FromResult(room);
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