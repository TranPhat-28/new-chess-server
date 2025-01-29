using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using new_chess_server.DTOs.LobbyDTO;

namespace new_chess_server.SignalR
{
    [Authorize]
    public class GameLobbyHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GameLobbyTracker _gameLobbyTracker;

        public GameLobbyHub(IHttpContextAccessor httpContextAccessor, GameLobbyTracker gameLobbyTracker)
        {
            _httpContextAccessor = httpContextAccessor;
            _gameLobbyTracker = gameLobbyTracker;
        }
        // Server invokes Client
        // public override async Task OnConnectedAsync()
        // {
        //     // Authed User Email
        //     var userEmail = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

        //     // Add the newly connected user to the online tracker
        //     await _onlineTracker.UserConnected(userEmail, Context.ConnectionId);

        //     // Send the list of online users
        //     var currentUsers = await _onlineTracker.GetOnlineUsers();
        //     await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        // }

        // Server invokes Client
        // public override async Task OnDisconnectedAsync(Exception? exception)
        // {
        //     // Authed User Email
        //     var userEmail = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

        //     // Disconnect the user from the online tracker
        //     await _onlineTracker.UserDisconnected(userEmail, Context.ConnectionId);

        //     // Send the list of online users
        //     var currentUsers = await _onlineTracker.GetOnlineUsers();
        //     await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

        //     await base.OnDisconnectedAsync(exception);
        // }

        // Client invokes Server
        public async Task<List<GameLobbyDto>> GetCurrentLobbyGameList()
        {
            // Return the current list of games
            return await _gameLobbyTracker.GetLobbyGameList();
        }

        // Create new room
        public async Task CreateGameRoom()
        {
            // Authed User ID and Name
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userName = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            await _gameLobbyTracker.CreateRoom(userId, userName);

            // Send live data
            var gameList = await _gameLobbyTracker.GetLobbyGameList();
            await Clients.All.SendAsync("NewRoomCreated", gameList);
        }
    }
}