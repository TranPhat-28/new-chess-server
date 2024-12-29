using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.Services.PracticeMode;

namespace new_chess_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PracticeModeController : ControllerBase
    {
        private readonly IPracticeModeService _practiceModeService;

        public PracticeModeController(IPracticeModeService practiceModeService)
        {
            _practiceModeService = practiceModeService;
        }

        [HttpGet("Saved")]
        public ActionResult<ServiceResponse<string>> GetPreviouslySavedGame()
        {
            var response = new ServiceResponse<string>
            {
                Data = "There is no saved game"
            };
            return response;
        }

        [HttpPost("Move")]
        public async Task<ActionResult<ServiceResponse<ResponseMoveDto>>> Move(RequestInputMoveDto requestInputMoveDto)
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
                    response = await _practiceModeService.Move(requestInputMoveDto);

                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[PracticeModeController] " + e.Message);

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