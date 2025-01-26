using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.SignalR
{
    public class MainConnectionHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("PlayerIsOnline", Context.UserIdentifier);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("PlayerIsOffline", Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }
    }
}