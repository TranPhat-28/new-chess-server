using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.UserProfileDTO
{
    public class FriendRequestActionDto
    {
        public int FriendRequestId { get; set; }
        public bool IsSender { get; set; }
        public bool IsReceiver { get; set; }
    }
}