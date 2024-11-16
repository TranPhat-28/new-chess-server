using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string ExternalID { get; set; } = "";
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Provider { get; set; } = "Google";
        public string DateJoined { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string SocialId { get; set; } = "";
        public GameStatistic? Statistic { get; set; }
        public List<User> Friends { get; set; } = new List<User>();
    }
}