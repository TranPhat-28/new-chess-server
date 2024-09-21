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

            var user = await _dataContext.Users.Include(u => u.FriendList).FirstOrDefaultAsync(u => u.Id == userId);


            if (user is null)
            {
                throw new Exception("Cannot find user");
            }

            if (user.FriendList is null)
            {
                return response;
            }

            var list = user.FriendList;

            response.Data = list.Select(f => _mapper.Map<FriendSummaryDetailDto>(f)).ToList();
            return response;
        }
    }
}