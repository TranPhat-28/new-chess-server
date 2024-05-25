using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using new_chess_server.Services.OAuth;
using new_chess_server.Data;

namespace new_chess_server.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOAuthService _oAuthService;
        private readonly DataContext _dataContext;

        public AuthenticationService(IOAuthService oAuthService, DataContext dataContext)
        {
            _oAuthService = oAuthService;
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<string>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto)
        {
            // Response
            var response = new ServiceResponse<string>();

            try
            {
                // Verify the token
                var result = await _oAuthService.VerifyGoogleIDToken(authenticationPostDto.Token);

                // Token is valid, perform register or login accordingly
                if (result.IsSuccess)
                {
                    // Check if user is in db
                    var user = await _dataContext.Users.FirstOrDefaultAsync(user => user.Email == result.Data!.Email && user.ExternalID == result.Data!.Sub);

                    // If already in db then return JWT
                    if (user is not null)
                    {
                        response.Data = "JWT";
                    }
                    // If not in db then Register
                    else
                    {
                        // Register by adding user to db
                        User newUser = new User()
                        {
                            Email = result.Data!.Email,
                            Name = result.Data!.Name,
                            ExternalID = result.Data!.Sub,
                            Picture = result.Data!.Picture
                        };

                        _dataContext.Users.Add(newUser);
                        await _dataContext.SaveChangesAsync();

                        // Then create and return a JWT
                        response.Data = "Register";
                    }
                }
                else
                {
                    Console.WriteLine("Error: ------------------------");
                    Console.WriteLine(result.Message);

                    response.IsSuccess = false;
                    response.Message = "We cannot connect you to our system right now.";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                response.IsSuccess = false;
                response.Message = "Interal server error";

                return response;
            }
        }

        private string CreateJWT()
        {
            return "JWT";
        }
    }
}