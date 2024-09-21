using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.FriendsDTO;
using new_chess_server.Services.Friends;

namespace new_chess_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<FriendSummaryDetailDto>>>> GetFriendList()
        {
            try
            {
                var response = new ServiceResponse<List<FriendSummaryDetailDto>>();
                response = await _friendsService.GetFriendList();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[FriendsController] " + e.Message);

                return StatusCode(500);
            }
        }
    }
}