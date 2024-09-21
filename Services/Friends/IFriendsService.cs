using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.FriendsDTO;

namespace new_chess_server.Services.Friends
{
    public interface IFriendsService
    {
        Task<ServiceResponse<List<FriendSummaryDetailDto>>> GetFriendList();
    }
}