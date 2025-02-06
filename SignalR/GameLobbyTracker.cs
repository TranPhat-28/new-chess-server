using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.LobbyDTO;

namespace new_chess_server.SignalR
{
    public class GameLobbyTracker
    {
        private static readonly List<GameRoom> GameList = new List<GameRoom>();

        public Task<string> CreateRoom(int hostId, string hostName, bool isPublicRoom, string roomPassword)
        {
            string uniqueId;

            lock (GameList)
            {
                // If key already existed, throw error
                if (GameList.Exists(room => room.Host.Id == hostId))
                {
                    throw new Exception("You cannot create more than one room");
                }
                else
                {
                    // Host info
                    var hostInfo = new RoomPlayer
                    {
                        Id = hostId,
                        Name = hostName

                    };

                    // Generate room ID
                    uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8); // 8-character unique ID

                    // Add the room
                    GameList.Add(new GameRoom
                    {
                        Id = uniqueId,
                        Host = hostInfo,
                        Player = null,
                        // Private room to be implemented
                        IsPlaying = false,
                        IsPrivate = false
                    });
                }
            }

            return Task.FromResult(uniqueId);
        }

        // public Task RemoveRoom(string roomId)
        // {
        //     lock (GameList)
        //     {
        //         var roomToRemove = GameList.Find(room => room.Id == roomId);
        //         if (roomToRemove is null)
        //         {
        //             Console.WriteLine($"Room ID {roomId} is not found on Game Lobby List");
        //         }
        //         else
        //         {
        //             GameList.Remove(roomToRemove);
        //         }
        //     }

        //     return Task.CompletedTask;
        // }

        public Task<List<GameRoom>> GetLobbyGameList()
        {
            var gameList = GameList;
            return Task.FromResult(gameList);
        }
    }
}