using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace new_chess_server.SignalR
{
    [Authorize]
    public class MainConnectionHub : Hub
    {
        private readonly OnlineTracker _onlineTracker;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GameLobbyTracker _gameLobbyTracker;
        private readonly IHubContext<GameLobbyHub> _gameLobbyHub;

        public MainConnectionHub(OnlineTracker onlineTracker, IHttpContextAccessor httpContextAccessor, GameLobbyTracker gameLobbyTracker, IHubContext<GameLobbyHub> gameLobbyHub)
        {
            _onlineTracker = onlineTracker;
            _httpContextAccessor = httpContextAccessor;
            _gameLobbyTracker = gameLobbyTracker;
            _gameLobbyHub = gameLobbyHub;
        }

        // Server invokes Client
        public override async Task OnConnectedAsync()
        {
            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Add the newly connected user to the online tracker
            await _onlineTracker.UserConnected(userId, Context.ConnectionId);

            // Send the list of online users
            var currentUsers = await _onlineTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        // Server invokes Client
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Disconnect the user from the online tracker
            await _onlineTracker.UserDisconnected(userId, Context.ConnectionId);

            // Remove game room created by this user if they are offline
            bool isPlayerOnline = await _onlineTracker.IsUserOnline(userId);
            if (!isPlayerOnline)
            {
                string removedRoomId = await _gameLobbyTracker.RemoveRoomByHostId(userId);
                await _gameLobbyHub.Clients.All.SendAsync("RoomRemoved", removedRoomId);
            }

            // Send the list of online users
            var currentUsers = await _onlineTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }

        // Client invokes Server
        public async Task<List<int>> GetCurrentOnlineFriends()
        {
            // Return the current list of online users
            return await _onlineTracker.GetOnlineUsers();
        }
    }
}