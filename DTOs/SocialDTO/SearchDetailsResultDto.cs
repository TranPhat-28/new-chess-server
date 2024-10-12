using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.UserProfileDTO;

namespace new_chess_server.DTOs.SocialDTO
{
    public class SearchDetailsResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Rank { get; set; } = "N/A";
        public bool IsFriend { get; set; }
        public FriendRequestActionDto? FriendRequestAction { get; set; } = null;
    }
}