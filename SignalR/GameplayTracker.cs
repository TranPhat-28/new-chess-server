using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}