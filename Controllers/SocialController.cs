using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.SocialDTO;
using new_chess_server.Services.Social;

namespace new_chess_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        [HttpGet("Search/{socialId}")]
        public async Task<ActionResult<ServiceResponse<SearchWithSocialIdResultDto>>> SearchWithSocialId(string socialId)
        {
            try
            {
                var response = new ServiceResponse<SearchWithSocialIdResultDto>();
                response = await _socialService.SearchWithSocialId(socialId);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpGet("Detail/{socialId}")]
        public async Task<ActionResult<ServiceResponse<SearchDetailsResultDto>>> SearchDetailWithSocialId(string socialId)
        {
            try
            {
                var response = new ServiceResponse<SearchDetailsResultDto>();
                response = await _socialService.SearchDetailWithSocialId(socialId);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpPost("Request")]
        public async Task<ActionResult<ServiceResponse<FriendRequest>>> SendFriendRequest(PostSendFriendRequestDto postSendFriendRequestDto)
        {
            if (postSendFriendRequestDto.Id == -1)
            {
                return BadRequest("Missing required field(s)");
            }
            // Search
            else
            {
                try
                {
                    var response = new ServiceResponse<FriendRequest>();
                    response = await _socialService.SendFriendRequest(postSendFriendRequestDto);
                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SocialController] " + e.Message);

                    return StatusCode(500);
                }
            }
        }

        [HttpDelete("Request/{requestId}")]
        public async Task<ActionResult<ServiceResponse<int>>> RemoveFriendRequest(int requestId)
        {
            try
            {
                var response = new ServiceResponse<int>();
                response = await _socialService.RemoveFriendRequest(requestId);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpPut("Request/{requestId}/Accept")]
        public async Task<ActionResult<ServiceResponse<string>>> AcceptFriendRequest(int requestId)
        {
            try
            {
                var response = new ServiceResponse<string>();
                response = await _socialService.AcceptFriendRequest(requestId);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
            // }
        }

        [HttpDelete("Friend/{socialId}")]
        public async Task<ActionResult<ServiceResponse<string>>> RemoveFriend(string socialId)
        {
            try
            {
                var response = new ServiceResponse<string>();
                response = await _socialService.RemoveFriend(socialId);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
        }
    }
}