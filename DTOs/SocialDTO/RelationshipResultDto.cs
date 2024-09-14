using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.SocialDTO
{
    public class RelationshipResultDto
    {
        public bool IsFriend { get; set; }
        public int? FriendRequestId { get; set; } = null;
        public bool IsRequestSender { get; set; }
        public bool IsRequestReceiver { get; set; }
    }
}