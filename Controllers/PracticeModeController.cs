using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.DTOs.PracticeModeDTO;
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

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckIfSavedGameExist()
        {
            try
            {
                var response = new ServiceResponse<bool>();
                response = await _practiceModeService.CheckIfSavedGameExist();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[PracticeModeController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpGet("Saved")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetSavedGameHistory()
        {
            try
            {
                var response = new ServiceResponse<List<string>>();
                response = await _practiceModeService.GetSavedGameHistory();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[PracticeModeController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpDelete("Saved")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteSavedGameHistory()
        {
            try
            {
                var response = new ServiceResponse<int>();
                response = await _practiceModeService.DeleteSavedGameHistory();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("[PracticeModeController] " + e.Message);

                return StatusCode(500);
            }
        }

        [HttpPost("Saved")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateSavedGameHistory(UpdateGameHistoryDto updateGameHistoryDto)
        {
            if (updateGameHistoryDto.Moves is null || updateGameHistoryDto.Moves.Count == 0)
            {
                return BadRequest("Missing required field(s)");
            }
            else
            {
                try
                {
                    var response = new ServiceResponse<int>();
                    response = await _practiceModeService.UpdateSavedGameHistory(updateGameHistoryDto);

                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[PracticeModeController] " + e.Message);

                    return StatusCode(500);
                }
            }
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