using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Data;
using new_chess_server.DTOs.FriendsDTO;

namespace new_chess_server.Services.Friends
{
    public class FriendsService : IFriendsService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FriendsService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<FriendSummaryDetailDto>>> GetFriendList()
        {
            var response = new ServiceResponse<List<FriendSummaryDetailDto>>();

            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _dataContext.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);


            if (user is null)
            {
                throw new Exception("Cannot find user");
            }

            if (user.Friends is null)
            {
                return response;
            }

            var list = user.Friends;

            response.Data = list.Select(f => _mapper.Map<FriendSummaryDetailDto>(f)).ToList();
            return response;
        }

        public async Task<ServiceResponse<FriendDetailsDto>> GetFriendDetails(int id)
        {
            var response = new ServiceResponse<FriendDetailsDto>();

            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Get Authed User info
            var user = await _dataContext.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new Exception("Cannot find user");
            }

            var list = user.Friends.ToList();

            var target = list.FirstOrDefault(t => t.Id == id);

            if (target is null)
            {
                throw new Exception("Cannot find target");
            }

            var data = new FriendDetailsDto
            {
                Id = target.Id,
                Name = target.Name,
                Picture = target.Picture
            };

            response.Data = data;
            return response;
        }
    }
}