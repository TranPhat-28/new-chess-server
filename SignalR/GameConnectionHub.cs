using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using new_chess_server.Data;
using new_chess_server.DTOs.MultiplayerModeDTO;

namespace new_chess_server.SignalR
{
    [Authorize]
    public class GameConnectionHub : Hub
    {
        private readonly GameLobbyTracker _gameLobbyTracker;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IHubContext<GameLobbyHub> _gameLobbyHub;
        private readonly GameplayTracker _gameplayTracker;
        private static readonly Dictionary<string, string> _connections = new();

        public GameConnectionHub(GameLobbyTracker gameLobbyTracker, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IHubContext<GameLobbyHub> gameLobbyHub, GameplayTracker gameplayTracker)
        {
            _gameLobbyTracker = gameLobbyTracker;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _gameLobbyHub = gameLobbyHub;
            _gameplayTracker = gameplayTracker;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var roomId = httpContext?.Request.Query["roomId"];

            if (string.IsNullOrEmpty(roomId))
            {
                throw new Exception("Room id is not defined");
            }

            // Save the connection id - room id so we can use it on OnDisconnectedAsync
            _connections[Context.ConnectionId] = roomId!;

            // Authed User ID and Name
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userName = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            var authUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (authUser is null)
            {
                throw new Exception("[GameLobbyHub] Cannot find user");
            }

            // If this room not exists yet, create new room
            var isRoomExist = await _gameLobbyTracker.CheckIfRoomExists(roomId!);
            if (isRoomExist == false)
            {
                await _gameLobbyTracker.CreateRoom(roomId!, authUser.Id, authUser.Name, authUser.SocialId, authUser.Picture, true, "");
            }
            else
            {
                await _gameLobbyTracker.JoinRoom(roomId!, authUser.Id, authUser.Name, authUser.SocialId, authUser.Picture, "");
            }

            // Send room list to Lobby Hub
            var gameList = await _gameLobbyTracker.GetLobbyGameList();
            await _gameLobbyHub.Clients.All.SendAsync("LobbyListUpdated", gameList);

            // Add player to group
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId!);

            // Send info to Room participants using Game Hub
            var roomInfo = await _gameLobbyTracker.GetRoomInfoById(roomId!);
            await Clients.Group(roomId!).SendAsync("UpdateRoomInfo", roomInfo);
        }

        // Server invokes Client
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var roomId))
            {
                // User who disconnected
                var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                // Send "Room disbanded" or "Player left" event to room
                var roomInfo = await _gameLobbyTracker.GetRoomInfoById(roomId!);
                if (roomInfo == null)
                {
                    throw new HubException("Cannot get room information");
                }
                else
                {
                    // Send "Room disbanded" or "Player left" event to room
                    if (userId == roomInfo.Host.Id)
                    {
                        // If Host left, send Disband event first
                        await Clients.Group(roomId).SendAsync("RoomDisbanded");
                        // Then remove the room from Room Tracker
                        await _gameLobbyTracker.RemoveRoomByRoomId(roomId);
                        // Send updated room list to Lobby Hub
                        var gameList = await _gameLobbyTracker.GetLobbyGameList();
                        await _gameLobbyHub.Clients.All.SendAsync("LobbyListUpdated", gameList);
                    }
                    else
                    {
                        // If Player left, remove player from Room
                        await _gameLobbyTracker.RemovePlayerFromRoom(roomId);
                        // Send updated room list to Lobby Hub
                        var gameList = await _gameLobbyTracker.GetLobbyGameList();
                        await _gameLobbyHub.Clients.All.SendAsync("LobbyListUpdated", gameList);
                        // Send Player left event with new room info
                        var newRoomInfo = await _gameLobbyTracker.GetRoomInfoById(roomId!);
                        await Clients.Group(roomId!).SendAsync("PlayerLeft", newRoomInfo);
                    }
                }

                _connections.Remove(Context.ConnectionId); // Remove connection from tracking
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task StartRoom(string roomId)
        {
            await _gameLobbyTracker.StartRoom(roomId);

            // Send room list to Lobby Hub
            var gameList = await _gameLobbyTracker.GetLobbyGameList();
            await _gameLobbyHub.Clients.All.SendAsync("LobbyListUpdated", gameList);

            // Get the Host and Player id
            var roomInfo = await _gameLobbyTracker.GetRoomInfoById(roomId);
            if (roomInfo is null || roomInfo.Player is null)
            {
                throw new Exception("[GameConnectionHub] Cannot start game session because either Room or Player is null");
            }
            // Start the Gameplay Tracker
            await _gameplayTracker.StartGameplay(roomId, roomInfo.Host.Id, roomInfo.Player.Id);
            // Send Game Start event to group
            await Clients.Group(roomId!).SendAsync("GameStarted");
            // Get whose turn
            var playerTurn = await _gameplayTracker.GetMovingPlayerId(roomId);
            // Send Waiting For Player Move event to group
            await Clients.Group(roomId!).SendAsync("WaitingForFirstPlayerMove", playerTurn);
        }

        public async Task PlayerMove(PlayerMoveDto playerMoveDto)
        {
            var update = await _gameplayTracker.MakeMove(playerMoveDto.RoomId, playerMoveDto.Move, playerMoveDto.PlayerId);
            // Send Next Move event to group
            await Clients.OthersInGroup(playerMoveDto.RoomId).SendAsync("NextMove", update);
        }
    }
}