using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Services.OAuth
{
    public class OAuthService : IOAuthService
    {
        public async Task<ServiceResponse<GoogleApiResponse>> VerifyGoogleIDToken(string googleIDToken)
        {
            try
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri("https://oauth2.googleapis.com")
                };

                // Make the request
                HttpResponseMessage response = await client.GetAsync($"tokeninfo?id_token={googleIDToken}");

                if (response.IsSuccessStatusCode)
                {
                    // Valid token
                    GoogleApiResponse? user = await response.Content.ReadFromJsonAsync<GoogleApiResponse>();
                    // Return the result
                    var result = new ServiceResponse<GoogleApiResponse>()
                    {
                        Data = user
                    };

                    return result;
                }
                else
                {
                    // Invalid token
                    GoogleApiErrorResponse? error = await response.Content.ReadFromJsonAsync<GoogleApiErrorResponse>();
                    // Return the result
                    var result = new ServiceResponse<GoogleApiResponse>()
                    {
                        IsSuccess = false,
                        Message = error!.Error_description,
                    };

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                var result = new ServiceResponse<GoogleApiResponse>()
                {
                    IsSuccess = false,
                    Message = e.Message,
                };

                return result;
            }
        }
    }
}