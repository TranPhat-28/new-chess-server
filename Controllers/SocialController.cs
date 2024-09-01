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
    }
}