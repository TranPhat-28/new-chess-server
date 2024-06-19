using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.UserProfileDTO
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Provider { get; set; } = "";
        public string DateJoined { get; set; } = "";
    }
}