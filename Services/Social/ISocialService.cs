using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.SocialDTO;

namespace new_chess_server.Services.Social
{
    public interface ISocialService
    {
        Task<ServiceResponse<SearchWithSocialIdResultDto>> SearchWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto);
        Task<ServiceResponse<SearchDetailsResultDto>> SearchDetailWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto);
    }
}