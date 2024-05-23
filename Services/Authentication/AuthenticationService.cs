using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace new_chess_server.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<ServiceResponse<string>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto)
        {
            // Verify the token
            var result = await VerifyIDToken(authenticationPostDto.Token);

            var response = new ServiceResponse<string>()
            {
                Data = result.Value
            };

            return response;
        }

        // Verify the user Token
        private static async Task<GetGoogleTokenVerification> VerifyIDToken(string token)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://oauth2.googleapis.com")
            };

            // Make the request
            HttpResponseMessage response = await client.GetAsync($"tokeninfo?id_token={token}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the successful response
                    GoogleApiResponse? user = await response.Content.ReadFromJsonAsync<GoogleApiResponse>();
                    var result = new GetGoogleTokenVerification()
                    {
                        ResponseType = GoogleApiResponseType.ValidToken,
                        Value = user!.Sub
                    };

                    return result;
                }
                else
                {
                    // Deserialize the error response
                    GoogleApiErrorResponse? error = await response.Content.ReadFromJsonAsync<GoogleApiErrorResponse>();
                    // Handle the error response
                    var result = new GetGoogleTokenVerification()
                    {
                        ResponseType = GoogleApiResponseType.InvalidToken,
                        Value = error!.Error_description
                    };

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var result = new GetGoogleTokenVerification()
                {
                    ResponseType = GoogleApiResponseType.InternalError,
                    Value = "Internal error"
                };

                return result;
            }

        }

        // Implement our own JWT here
    }
}