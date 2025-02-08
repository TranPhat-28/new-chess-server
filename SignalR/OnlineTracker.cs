using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.SignalR
{
    public class OnlineTracker
    {
        // Dictionary
        // Int: store connecting user's Id
        // List<string>: store all connection of that specific user. A user can connect from multiple
        // devices
        private static readonly Dictionary<int, List<string>> OnlineUsers = new Dictionary<int, List<string>>();

        public Task UserConnected(int userId, string connectionId)
        {
            lock (OnlineUsers)
            {
                // If key already existed, add a new connection id
                if (OnlineUsers.ContainsKey(userId))
                {
                    OnlineUsers[userId].Add(connectionId);
                }
                // First connection, add key
                else
                {
                    OnlineUsers.Add(userId, new List<string> { connectionId });
                }
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnected(int userId, string connectionId)
        {
            lock (OnlineUsers)
            {
                // If no connection
                if (!OnlineUsers.ContainsKey(userId))
                {
                    return Task.CompletedTask;
                }

                // Remove connection
                OnlineUsers[userId].Remove(connectionId);

                // If count is 0 remove the key
                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<int>> GetOnlineUsers()
        {
            List<int> onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToList();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<bool> IsUserOnline(int userId)
        {
            lock (OnlineUsers)
            {
                return Task.FromResult(OnlineUsers.ContainsKey(userId));
            }
        }
    }
}