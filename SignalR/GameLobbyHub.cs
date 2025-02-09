using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using new_chess_server.Data;
using new_chess_server.DTOs.LobbyDTO;

namespace new_chess_server.SignalR
{
    [Authorize]
    public class GameLobbyHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GameLobbyTracker _gameLobbyTracker;
        private readonly DataContext _dataContext;

        public GameLobbyHub(IHttpContextAccessor httpContextAccessor, GameLobbyTracker gameLobbyTracker, DataContext dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _gameLobbyTracker = gameLobbyTracker;
            _dataContext = dataContext;
        }

        // Client invokes Server
        public async Task<List<GameRoom>> GetCurrentLobbyGameList()
        {
            // Return the current list of games
            return await _gameLobbyTracker.GetLobbyGameList();
        }

        // Create new room
        public async Task<string> CreateGameRoom(CreateGameRoomDto createGameRoomDto)
        {
            // Authed User ID and Name
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userName = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

            var authUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (authUser is null)
            {
                throw new Exception("[GameLobbyHub] Cannot find user");
            }

            string newRoomId = await _gameLobbyTracker.CreateRoom(userId, userName, authUser.SocialId, authUser.Picture, createGameRoomDto.IsPublicRoom, createGameRoomDto.RoomPassword);

            // Send live data
            var gameList = await _gameLobbyTracker.GetLobbyGameList();
            await Clients.All.SendAsync("NewRoomCreated", gameList);

            return newRoomId;
        }
    }
}