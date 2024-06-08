using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.Services.QuickPlay;

namespace new_chess_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuickPlayController : ControllerBase
    {
        private readonly IQuickPlayHandlerService _quickPlayHandlerService;

        public QuickPlayController(IQuickPlayHandlerService quickPlayHandlerService)
        {
            _quickPlayHandlerService = quickPlayHandlerService;
        }

        [HttpGet("GetPlayerRandomId")]
        public ActionResult<ServiceResponse<int>> GetPlayerRandomId()
        {
            Random rnd = new();
            // Random between 10000 and 99999
            int PlayerId = rnd.Next(10000, 100000);

            var response = new ServiceResponse<int>
            {
                Data = PlayerId
            };

            return response;
        }

        [HttpPost("Move")]
        public ActionResult<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto)
        {
            if (requestInputMoveDto.Fen == "")
            {
                return BadRequest("Missing required field(s)");
            }
            // Validate FEN string
            else if (!FenIsValid(requestInputMoveDto.Fen))
            {
                return BadRequest("Invalid FEN string");
            }
            else
            {
                try
                {
                    var response = new ServiceResponse<ResponseMoveDto>();
                    response = _quickPlayHandlerService.Move(requestInputMoveDto);

                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[QuickPlayController] " + e.Message);

                    return StatusCode(500);
                }
            }
        }

        private static bool FenIsValid(string inputFen)
        {
            // Regular expression pattern for FEN validation
            string pattern = @"^([rnbqkpRNBQKP1-8]{1,8}\/){7}[rnbqkpRNBQKP1-8]{1,8} [wb] [-KQkq]{1,4} ([a-h][1-8]|-) \d+ \d+$";

            return Regex.IsMatch(inputFen, pattern);
        }
    }
}