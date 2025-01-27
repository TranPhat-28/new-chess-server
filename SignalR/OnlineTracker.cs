using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.SignalR
{
    public class OnlineTracker
    {
        // Dictionary
        // String: store user's email
        // List<string>: store all connection of that specific user. A user can connect from multiple
        // devices
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task UserConnected(string email, string connectionId)
        {
            lock (OnlineUsers)
            {
                // If key already existed, add a new connection id
                if (OnlineUsers.ContainsKey(email))
                {
                    OnlineUsers[email].Add(connectionId);
                }
                // First connection, add key
                else
                {
                    OnlineUsers.Add(email, new List<string> { connectionId });
                }
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnected(string email, string connectionId)
        {
            lock (OnlineUsers)
            {
                // If no connection
                if (!OnlineUsers.ContainsKey(email))
                {
                    return Task.CompletedTask;
                }

                // Remove connection
                OnlineUsers[email].Remove(connectionId);

                // If count is 0 remove the key
                if (OnlineUsers[email].Count == 0)
                {
                    OnlineUsers.Remove(email);
                }
            }

            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }
    }
}