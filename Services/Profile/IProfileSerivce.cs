using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.UserProfileDTO;

namespace new_chess_server.Services.Profile
{
    public interface IProfileSerivce
    {
        Task<ServiceResponse<UserProfileDto>> GetUserProfile();
    }
}