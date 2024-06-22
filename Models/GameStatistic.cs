using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Models
{
    public class GameStatistic
    {
        public int Id { get; set; }
        public int Ranking { get; set; } = 0;
        public int PracticePlayedEasy { get; set; } = 0;
        public int PracticeVictoryEasy { get; set; } = 0;
        public int PracticePlayedMedium { get; set; } = 0;
        public int PracticeVictoryMedium { get; set; } = 0;
        public int PracticePlayedHard { get; set; } = 0;
        public int PracticeVictoryHard { get; set; } = 0;
        public int OnlinePlayedEasy { get; set; } = 0;
        public int OnlineVictoryEasy { get; set; } = 0;
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}