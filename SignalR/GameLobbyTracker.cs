using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.LobbyDTO;

namespace new_chess_server.SignalR
{
    public class GameLobbyTracker
    {
        private static readonly List<GameLobbyDto> GameList = new List<GameLobbyDto>();

        public Task CreateRoom(int hostId, string hostName)
        {
            lock (GameList)
            {
                // If key already existed, throw error
                if (GameList.Exists(room => room.HostId == hostId))
                {
                    throw new Exception("You cannot create more than one room");
                }
                else
                {
                    string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8); // 8-character unique ID
                    GameList.Add(new GameLobbyDto
                    {
                        Id = uniqueId,
                        HostName = hostName,
                        HostId = hostId
                    });
                }
            }

            return Task.CompletedTask;
        }

        public Task RemoveRoom(string roomId)
        {
            lock (GameList)
            {
                var roomToRemove = GameList.Find(room => room.Id == roomId);
                if (roomToRemove is null)
                {
                    Console.WriteLine($"Room ID {roomId} is not found on Game Lobby List");
                }
                else
                {
                    GameList.Remove(roomToRemove);
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<GameLobbyDto>> GetLobbyGameList()
        {
            var gameList = GameList;
            return Task.FromResult(gameList);
        }
    }
}