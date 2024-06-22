using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.DTOs.UserProfileDTO
{
    public class UserGameStatisticDto
    {
        public int Ranking { get; set; } = 0;
        public int PracticePlayedEasy { get; set; } = 0;
        public int PracticeVictoryEasy { get; set; } = 0;
        public int PracticePlayedMedium { get; set; } = 0;
        public int PracticeVictoryMedium { get; set; } = 0;
        public int PracticePlayedHard { get; set; } = 0;
        public int PracticeVictoryHard { get; set; } = 0;
        public int OnlinePlayed { get; set; } = 0;
        public int OnlineVictory { get; set; } = 0;
    }
}