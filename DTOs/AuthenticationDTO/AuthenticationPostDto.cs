using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.AuthenticationDTO
{
    public class AuthenticationPostDto
    {
        public string Provider { get; set; } = "Google";
        public string Token { get; set; } = "";
    }
}