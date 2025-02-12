using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using new_chess_server.Data;

namespace new_chess_server.SignalR
{
    [Authorize]
    public class GameConnectionHub : Hub
    {
        private readonly GameLobbyTracker _gameLobbyTracker;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IHubContext<GameLobbyHub> _gameLobbyHub;

        public GameConnectionHub(GameLobbyTracker gameLobbyTracker, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IHubContext<GameLobbyHub> gameLobbyHub)
        {
            _gameLobbyTracker = gameLobbyTracker;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _gameLobbyHub = gameLobbyHub;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var roomId = httpContext?.Request.Query["roomId"];

            if (string.IsNullOrEmpty(roomId))
            {
                throw new Exception("Room id is not defined");
            }

            // Authed User ID and Name
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userName = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            var authUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (authUser is null)
            {
                throw new Exception("[GameLobbyHub] Cannot find user");
            }

            await _gameLobbyTracker.CreateRoom(roomId!, authUser.Id, authUser.Name, authUser.SocialId, authUser.Picture, true, "");
            // Send room list
            var gameList = await _gameLobbyTracker.GetLobbyGameList();
            await _gameLobbyHub.Clients.All.SendAsync("NewRoomCreated", gameList);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId!);
        }

        // Server invokes Client
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}