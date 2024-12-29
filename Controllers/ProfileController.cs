using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.UserProfileDTO;
using new_chess_server.Services.Profile;

namespace new_chess_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileSerivce _profileSerivce;

        public ProfileController(IProfileSerivce profileSerivce)
        {
            _profileSerivce = profileSerivce;
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<ServiceResponse<UserProfileDto>>> GetUserProfile()
        {
            try
            {
                var response = new ServiceResponse<UserProfileDto>();
                response = await _profileSerivce.GetUserProfile();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ProfileController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpGet("Statistic")]
        public async Task<ActionResult<ServiceResponse<UserGameStatisticDto>>> GetUserGameStatistic()
        {
            try
            {
                var response = new ServiceResponse<UserGameStatisticDto>();
                response = await _profileSerivce.GetUserGameStatistic();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ProfileController] " + e.Message);

                return StatusCode(500);
            }
        }
    }
}