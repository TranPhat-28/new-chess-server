using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace new_chess_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Google")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto)
        {
            if (authenticationPostDto.Provider == "" || authenticationPostDto.Token == "")
            {
                return BadRequest("Missing required field(s)");
            }
            else
            {
                var response = await _authenticationService.LoginWithGoogle(authenticationPostDto);
                return response;
            }
        }
    }
}