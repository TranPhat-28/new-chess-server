using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Data;
using new_chess_server.DTOs.UserProfileDTO;

namespace new_chess_server.Services.Profile
{
    public class ProfileService : IProfileSerivce
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ProfileService(IHttpContextAccessor httpContextAccessor, DataContext dataContext, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<UserProfileDto>> GetUserProfile()
        {
            var response = new ServiceResponse<UserProfileDto>();

            // The user id taken from the JWT token
            int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Query from DB
            var user = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user is null)
            {
                throw new Exception("Cannot find user in DB");
            }

            response.Data = _mapper.Map<UserProfileDto>(user);

            return response;
        }
    }
}