using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Data;
using new_chess_server.DTOs.LobbyDTO;

namespace new_chess_server.SignalR
{
    public class GameLobbyTracker
    {
        private static readonly List<GameRoom> GameList = new List<GameRoom>();
        public Task CreateRoom(string roomId, int hostId, string hostName, string hostSocialId, string hostProfilePicture, bool isPublicRoom, string roomPassword)
        {
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
                        Name = hostName,
                        Picture = hostProfilePicture,
                        SocialId = hostSocialId
                    };

                    // Add the room
                    GameList.Add(new GameRoom
                    {
                        Id = roomId,
                        Host = hostInfo,
                        Player = null,
                        // Private room to be implemented
                        IsPlaying = false,
                        IsPrivate = false
                    });
                }
            }

            return Task.CompletedTask;
        }

        public Task RemoveRoomByRoomId(string roomId)
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

        public Task RemoveRoomByHostId(int hostId)
        {
            lock (GameList)
            {
                var roomToRemove = GameList.Find(room => room.Host.Id == hostId);
                if (roomToRemove is null)
                {
                    Console.WriteLine($"[GameLobbyTracker] All rooms created by player {hostId} have been removed");
                }
                else
                {
                    GameList.Remove(roomToRemove);
                }

                return Task.CompletedTask;
            }
        }

        public Task<List<GameRoom>> GetLobbyGameList()
        {
            lock (GameList)
            {
                return Task.FromResult(GameList.ToList()); // Return a new list to avoid unintended modifications
            }
        }

        public Task<GameRoom?> GetRoomInfoById(string roomId)
        {
            lock (GameList)
            {
                return Task.FromResult(GameList.FirstOrDefault(room => room.Id == roomId));
            }
        }

        public Task<bool> CheckIfRoomExists(string roomId)
        {
            lock (GameList)
            {
                return Task.FromResult(GameList.Any(room => room.Id == roomId));
            }
        }

        public Task JoinRoom(string roomId, int playerId, string playerName, string playerSocialId, string playerProfilePicture, string roomPassword)
        {
            lock (GameList)
            {
                // Player info
                var playerInfo = new RoomPlayer
                {
                    Id = playerId,
                    Name = playerName,
                    Picture = playerProfilePicture,
                    SocialId = playerSocialId
                };

                // Add to the room
                var room = GameList.FirstOrDefault(r => r.Id == roomId);
                if (room != null)
                {
                    room.Player = playerInfo;
                }
                else
                {
                    throw new Exception("Cannot find room to join");
                }
            }

            return Task.CompletedTask;
        }

        public Task StartRoom(string roomId)
        {
            lock (GameList)
            {
                var room = GameList.FirstOrDefault(r => r.Id == roomId);
                if (room != null)
                {
                    room.IsPlaying = true;
                }
                else
                {
                    throw new Exception("Cannot find room to join");
                }
            }

            return Task.CompletedTask;
        }
    }
}