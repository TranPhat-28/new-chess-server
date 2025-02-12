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
    }
}