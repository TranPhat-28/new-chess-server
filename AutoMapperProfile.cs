using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.SocialDTO;
using new_chess_server.DTOs.UserProfileDTO;

namespace new_chess_server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserProfileDto>();
            CreateMap<GameStatistic, UserGameStatisticDto>();
            CreateMap<User, SearchWithSocialIdResultDto>();
        }
    }
}