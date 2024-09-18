using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.SocialDTO;

namespace new_chess_server.Services.Social
{
    public interface ISocialService
    {
        Task<ServiceResponse<SearchWithSocialIdResultDto>> SearchWithSocialId(string socialId);
        Task<ServiceResponse<SearchDetailsResultDto>> SearchDetailWithSocialId(string socialId);
        Task<ServiceResponse<RelationshipResultDto>> GetRelationship(string socialId);
        // IMPROVEMENT: Check if Targer has already been a friend, if yes, throw error and do not create request
        Task<ServiceResponse<FriendRequest>> SendFriendRequest(PostSendFriendRequestDto postSendFriendRequestDto);
        Task<ServiceResponse<int>> RemoveFriendRequest(int requestId);
        Task<ServiceResponse<string>> AcceptFriendRequest(int requestId);
        Task<ServiceResponse<string>> RemoveFriend();
    }
}