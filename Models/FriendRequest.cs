using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}