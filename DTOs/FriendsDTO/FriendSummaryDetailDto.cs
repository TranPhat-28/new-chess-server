using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.FriendsDTO
{
    public class FriendSummaryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
    }
}