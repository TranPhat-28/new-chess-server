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
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        [HttpPost("Search")]
        public async Task<ActionResult<ServiceResponse<SearchWithSocialIdResultDto>>> SearchWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto)
        {
            if (getSearchWithSocialIdDto.SocialId == "")
            {
                return BadRequest("Missing required field(s)");
            }
            // Search
            else
            {
                try
                {
                    var response = new ServiceResponse<SearchWithSocialIdResultDto>();
                    response = await _socialService.SearchWithSocialId(getSearchWithSocialIdDto);

                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SocialController] " + e.Message);

                    return StatusCode(500);
                }
            }
        }

        [HttpPost("Detail")]
        public async Task<ActionResult<ServiceResponse<SearchDetailsResultDto>>> SearchDetailWithSocialId(GetSearchWithSocialIdDto getSearchWithSocialIdDto)
        {
            if (getSearchWithSocialIdDto.SocialId == "")
            {
                return BadRequest("Missing required field(s)");
            }
            // Search
            else
            {
                try
                {
                    var response = new ServiceResponse<SearchDetailsResultDto>();
                    response = await _socialService.SearchDetailWithSocialId(getSearchWithSocialIdDto);
                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SocialController] " + e.Message);

                    return StatusCode(500);
                }
            }

        }

        [HttpGet("Relationship/{socialId}")]
        public async Task<ActionResult<ServiceResponse<RelationshipResultDto>>> GetRelationship(string socialId)
        {
            try
            {
                var response = new ServiceResponse<RelationshipResultDto>();
                response = await _socialService.GetRelationship(socialId);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpPost("Request/Create")]
        public async Task<ActionResult<ServiceResponse<string>>> SendFriendRequest()
        {
            // if ()
            // {
            //     return BadRequest("Missing required field(s)");
            // }
            // else
            // {
            try
            {
                var response = new ServiceResponse<string>();
                response = await _socialService.SendFriendRequest();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
            // }
        }

        [HttpPost("Request/Remove")]
        public async Task<ActionResult<ServiceResponse<string>>> RemoveFriendRequest()
        {
            // if ()
            // {
            //     return BadRequest("Missing required field(s)");
            // }
            // else
            // {
            try
            {
                var response = new ServiceResponse<string>();
                response = await _socialService.RemoveFriendRequest();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
            // }
        }

        [HttpDelete("Friend")]
        public async Task<ActionResult<ServiceResponse<string>>> RemoveFriend()
        {
            // if ()
            // {
            //     return BadRequest("Missing required field(s)");
            // }
            // else
            // {
            try
            {
                var response = new ServiceResponse<string>();
                response = await _socialService.RemoveFriend();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocialController] " + e.Message);

                return StatusCode(500);
            }
            // }
        }
    }
}