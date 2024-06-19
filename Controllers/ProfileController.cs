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
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileSerivce _profileSerivce;

        public ProfileController(IProfileSerivce profileSerivce)
        {
            _profileSerivce = profileSerivce;
        }

        [Authorize]
        [HttpGet("GetProfile")]
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
    }
}