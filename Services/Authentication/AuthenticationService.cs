using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using new_chess_server.Services.OAuth;

namespace new_chess_server.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOAuthService _oAuthService;

        public AuthenticationService(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public async Task<ServiceResponse<string>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto)
        {
            // Verify the token
            var result = await _oAuthService.VerifyGoogleIDToken(authenticationPostDto.Token);

            if (result.IsSuccess)
            {
                Console.WriteLine(result.Data!.Name);
            }
            else
            {
                Console.WriteLine("Error: ------------------------");
                Console.WriteLine(result.Message);
            }

            var response = new ServiceResponse<string>();

            return response;
        }
    }
}