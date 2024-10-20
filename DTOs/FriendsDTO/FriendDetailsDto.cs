using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.FriendsDTO
{
    public class FriendDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Rank { get; set; } = "N/A";
    }
}