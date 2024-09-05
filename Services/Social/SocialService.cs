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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SocialService(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<SearchDetailsResultDto>> SearchDetailWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto)
        {
            var response = new ServiceResponse<SearchDetailsResultDto>();

            // Search for target user detail
            var result = await _dataContext.Users.FirstOrDefaultAsync(user => user.SocialId == getSearchWithSocialIdDto.SocialId);

            if (result is null)
            {
                response.IsSuccess = false;
                response.Message = "Cannot find user information";
            }
            else
            {
                SearchDetailsResultDto data = new SearchDetailsResultDto
                {
                    Name = result.Name,
                    Picture = result.Picture,
                    IsFriend = false
                };

                // Authed User ID
                var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // Search for friend status with current user
                var userData = await _dataContext.Users.Include(u => u.FriendList).FirstOrDefaultAsync(u => u.Id == userId);

                // Look for friend status with target search
                if (userData!.FriendList is null)
                {
                    data.IsFriend = false;
                }
                else
                {
                    var friendStatus = userData!.FriendList.FirstOrDefault(f => f.SocialId == getSearchWithSocialIdDto.SocialId);

                    if (friendStatus is null)
                    {
                        data.IsFriend = false;
                    }
                    else
                    {
                        data.IsFriend = true;
                    }
                }

                response.Data = data;
            }

            return response;
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

        public async Task<ServiceResponse<string>> SendFriendRequest()
        {
            var response = new ServiceResponse<string>
            {
                Data = "Friend Request Created"
            };

            return response;
        }

        public async Task<ServiceResponse<string>> RemoveFriendRequest()
        {
            var response = new ServiceResponse<string>
            {
                Data = "Friend Request Removed"
            };

            return response;
        }

        public async Task<ServiceResponse<string>> RemoveFriend()
        {
            var response = new ServiceResponse<string>
            {
                Data = "Remove Friend"
            };

            return response;
        }
    }
}