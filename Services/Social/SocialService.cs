using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Data;
using new_chess_server.DTOs.SocialDTO;

namespace new_chess_server.Services.Social
{
    public class SocialService : ISocialService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public SocialService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<SearchWithSocialIdResultDto>> SearchWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto)
        {
            var response = new ServiceResponse<SearchWithSocialIdResultDto>();
            var result = await _dataContext.Users.FirstOrDefaultAsync(user => user.SocialId == getSearchWithSocialIdDto.SocialId);

            if (result is not null)
            {
                response.Data = _mapper.Map<SearchWithSocialIdResultDto>(result);
            }
            else
            {
                response.Data = null;
            }

            return response;
        }
    }
}