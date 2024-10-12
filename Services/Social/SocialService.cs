using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Data;
using new_chess_server.DTOs.SocialDTO;
using new_chess_server.DTOs.UserProfileDTO;

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

        public async Task<ServiceResponse<SearchDetailsResultDto>> SearchDetailWithSocialId(string socialId)
        {
            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = new ServiceResponse<SearchDetailsResultDto>();
            var responseData = new SearchDetailsResultDto();

            // Get detail information
            var target = await _dataContext.Users.FirstOrDefaultAsync(u => u.SocialId == socialId);

            if (target is null)
            {
                response.IsSuccess = false;
                response.Message = "Cannot find player";
                return response;
            }

            // Get friend status
            var isFriend = await IsFriend(userId, target.Id);

            // Social actions status
            if (isFriend == true)
            {
                responseData.FriendRequestAction = null;
            }
            else
            {
                responseData.FriendRequestAction = await GetFriendRequestActionState(userId, target.Id);
            }

            responseData.Id = target.Id;
            responseData.Name = target.Name;
            responseData.Picture = target.Picture;
            responseData.IsFriend = isFriend;

            response.Data = responseData;
            return response;
        }

        public async Task<ServiceResponse<SearchWithSocialIdResultDto>> SearchWithSocialId(string socialId)
        {
            var response = new ServiceResponse<SearchWithSocialIdResultDto>();
            var result = await _dataContext.Users.FirstOrDefaultAsync(user => user.SocialId == socialId);

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

        public async Task<ServiceResponse<FriendRequest>> SendFriendRequest(PostSendFriendRequestDto postSendFriendRequestDto)
        {
            var response = new ServiceResponse<FriendRequest>();

            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Get target
            var target = await _dataContext.Users.FirstOrDefaultAsync(t => t.Id == postSendFriendRequestDto.Id);

            // If target is null
            if (target is null)
            {
                response.Message = "Cannot find target player";
                response.IsSuccess = false;
                return response;
            }
            var targetId = target.Id;

            // Check if already friend
            bool isFriend = await IsFriend(userId, targetId);
            if (isFriend == true)
            {
                response.Message = "This player is already your friend";
                response.IsSuccess = false;
                return response;
            }

            // Check for duplicate request
            var duplicate = await _dataContext.FriendRequests.FirstOrDefaultAsync(r => r.SenderId == userId && r.ReceiverId == targetId);

            if (duplicate is not null)
            {
                response.IsSuccess = false;
                response.Message = "Friend request already sent";
                return response;
            }

            // Create the new Friend request
            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = userId,
                ReceiverId = targetId
            };

            _dataContext.FriendRequests.Add(friendRequest);
            await _dataContext.SaveChangesAsync();

            // Response
            response.Data = friendRequest;
            response.Message = "Friend request has been sent";
            return response;
        }

        public async Task<ServiceResponse<int>> RemoveFriendRequest(int requestId)
        {
            var response = new ServiceResponse<int>();

            var request = await _dataContext.FriendRequests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request is null)
            {
                throw new Exception("Cannot find friend request");
            }

            _dataContext.FriendRequests.Remove(request);
            await _dataContext.SaveChangesAsync();

            response.Data = request.Id;
            response.Message = "Request has been cancelled";
            return response;
        }

        public async Task<ServiceResponse<string>> AcceptFriendRequest(int requestId)
        {
            var response = new ServiceResponse<string>();

            // Get the request from db
            var request = await _dataContext.FriendRequests.FirstOrDefaultAsync(r => r.Id == requestId);

            // If reuqest is null
            if (request is null)
            {
                throw new Exception("Cannot find friend request");
            }

            // Get the sender and receiver
            var sender = await _dataContext.Users.FirstOrDefaultAsync(s => s.Id == request.SenderId);
            var receiver = await _dataContext.Users.FirstOrDefaultAsync(r => r.Id == request.ReceiverId);

            if (sender is null || receiver is null)
            {
                throw new Exception("Cannot add undefined player as friend");
            }

            // Perform add friend
            sender.FriendList.Add(receiver);
            receiver.FriendList.Add(sender);

            // Then remove the friend request
            _dataContext.FriendRequests.Remove(request);

            await _dataContext.SaveChangesAsync();

            response.Message = "Player has been added as friend";

            return response;
        }

        public async Task<ServiceResponse<string>> RemoveFriend(string socialId)
        {
            var response = new ServiceResponse<string>();

            // Authed User ID
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Get both user
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var target = await _dataContext.Users.FirstOrDefaultAsync(t => t.SocialId == socialId);

            if (user is null || target is null)
            {
                throw new Exception("Cannot find user");
            }

            user.FriendList.Remove(target);
            target.FriendList.Remove(user);

            await _dataContext.SaveChangesAsync();

            response.Data = socialId;
            response.Message = "Friend removed successfully";

            return response;
        }

        public async Task<bool> IsFriend(int userId, int targetId)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user!.FriendList.Any(t => t.Id == targetId);
        }

        private async Task<FriendRequestActionDto?> GetFriendRequestActionState(int userId, int targetId)
        {
            var request = await _dataContext.FriendRequests.FirstOrDefaultAsync(req =>
                (req.SenderId == userId && req.ReceiverId == targetId) ||
                (req.SenderId == targetId && req.ReceiverId == userId)
            );

            if (request is null)
            {
                return null;
            }
            else
            {
                var response = new FriendRequestActionDto
                {
                    FriendRequestId = request.Id,
                    IsSender = request.SenderId == userId,
                    IsReceiver = request.ReceiverId == userId
                };

                return response;
            }
        }
    }
}