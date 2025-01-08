using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class PracticeModeGameHistory
    {
        public int Id { get; set; }
        public List<string>? Moves { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}