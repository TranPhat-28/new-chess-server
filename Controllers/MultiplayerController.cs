using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.MultiplayerModeDTO;
using new_chess_server.Services.Multiplayer;

namespace new_chess_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MultiplayerController : ControllerBase
    {
        private readonly IMultiplayerService _multiplayerService;

        public MultiplayerController(IMultiplayerService multiplayerService)
        {
            _multiplayerService = multiplayerService;
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<ServiceResponse<GameRoomDto>>> GetRoomInfo(string roomId)
        {
            try
            {
                var response = new ServiceResponse<GameRoomDto>();
                response = await _multiplayerService.GetRoomInfoById(roomId);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[MultiplayerController] " + e.Message);

                return StatusCode(500);
            }
        }
    }
}