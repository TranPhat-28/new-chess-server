using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.SignalR
{
    public class MainConnectionHub : Hub
    {
        private readonly OnlineTracker _onlineTracker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MainConnectionHub(OnlineTracker onlineTracker, IHttpContextAccessor httpContextAccessor)
        {
            _onlineTracker = onlineTracker;
            _httpContextAccessor = httpContextAccessor;
        }

        // Server invokes Client
        public override async Task OnConnectedAsync()
        {
            // Authed User Email
            var userEmail = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            // Add the newly connected user to the online tracker
            await _onlineTracker.UserConnected(userEmail, Context.ConnectionId);

            // Send the list of online users
            var currentUsers = await _onlineTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        // Server invokes Client
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Authed User Email
            var userEmail = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            // Disconnect the user from the online tracker
            await _onlineTracker.UserDisconnected(userEmail, Context.ConnectionId);

            // Send the list of online users
            var currentUsers = await _onlineTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }

        // Client invokes Server
        public async Task<string[]> GetCurrentOnlineFriends()
        {
            // Return the current list of online users
            return await _onlineTracker.GetOnlineUsers();
        }
    }
}